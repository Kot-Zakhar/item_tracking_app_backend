provider "aws" {
  region = var.region
}

data "terraform_remote_state" "eks" {
  backend = "s3"
  config = {
    bucket = var.eks_state_bucket
    key    = var.eks_state_key
    region = var.region
  }
}

locals {
  name              = "${var.project}-${var.env}"
  oidc_provider_arn = data.terraform_remote_state.eks.outputs.oidc_provider_arn
  oidc_issuer_url   = data.terraform_remote_state.eks.outputs.cluster_oidc_issuer_url
  oidc_hostpath     = replace(local.oidc_issuer_url, "https://", "")

  base_tags = merge(
    {
      Project     = var.project
      Environment = var.env
      ManagedBy   = "terraform"
    },
    var.tags
  )
}

# ECR
locals {
  ecr_repos = [
    "ittrap-api-gateway",
    "ittrap-user-service",
    "ittrap-identity-service",
    "ittrap-email-service",
    "ittrap-query-service",
    "ittrap-inventory-service",
    "ittrap-location-service",
    "ittrap-management-service",
  ]
}

resource "aws_ecr_repository" "repos" {
  for_each = toset(local.ecr_repos)
  name     = each.value

  image_scanning_configuration {
    scan_on_push = true
  }

  tags = local.base_tags
}

# SNS/SQS (names match your existing Localstack naming)
locals {
  sns_topics = [
    "user-events",
    "auth-events",
    "item-events",
    "location-events",
    "management-events",
  ]

  sqs_queues = [
    "auth-queue",
    "query-queue",
    "management-queue",
  ]
}

resource "aws_sns_topic" "topics" {
  for_each = toset(local.sns_topics)
  name     = "${local.name}-${each.value}"
  tags     = local.base_tags
}

resource "aws_sqs_queue" "queues" {
  for_each = toset(local.sqs_queues)
  name     = "${local.name}-${each.value}"
  tags     = local.base_tags
}

# IRSA (example policy for SNS publish + SQS receive/delete)
data "aws_iam_policy_document" "sqs_sns_access" {
  statement {
    sid    = "SqsAccess"
    effect = "Allow"
    actions = [
      "sqs:ReceiveMessage",
      "sqs:DeleteMessage",
      "sqs:GetQueueAttributes",
      "sqs:GetQueueUrl",
      "sqs:ChangeMessageVisibility",
    ]
    resources = [for q in aws_sqs_queue.queues : q.arn]
  }

  statement {
    sid    = "SnsPublish"
    effect = "Allow"
    actions = [
      "sns:Publish",
    ]
    resources = [for t in aws_sns_topic.topics : t.arn]
  }
}

resource "aws_iam_policy" "ittrap_messaging" {
  name   = "${local.name}-messaging"
  policy = data.aws_iam_policy_document.sqs_sns_access.json
  tags   = local.base_tags
}

locals {
  irsa_subjects = [
    for sa in var.irsa_service_account_names :
    "system:serviceaccount:${var.k8s_namespace}:${sa}"
  ]
}

data "aws_iam_policy_document" "irsa_assume_role" {
  statement {
    effect  = "Allow"
    actions = ["sts:AssumeRoleWithWebIdentity"]

    principals {
      type        = "Federated"
      identifiers = [local.oidc_provider_arn]
    }

    condition {
      test     = "StringEquals"
      variable = "${local.oidc_hostpath}:aud"
      values   = ["sts.amazonaws.com"]
    }

    condition {
      test     = "StringLike"
      variable = "${local.oidc_hostpath}:sub"
      values   = local.irsa_subjects
    }
  }
}

resource "aws_iam_role" "ittrap_irsa_messaging" {
  name               = "${local.name}-irsa-messaging"
  assume_role_policy = data.aws_iam_policy_document.irsa_assume_role.json
  tags               = local.base_tags
}

resource "aws_iam_role_policy_attachment" "ittrap_irsa_messaging" {
  role       = aws_iam_role.ittrap_irsa_messaging.name
  policy_arn = aws_iam_policy.ittrap_messaging.arn
}

output "eks_cluster_name" {
  value = data.terraform_remote_state.eks.outputs.cluster_name
}
