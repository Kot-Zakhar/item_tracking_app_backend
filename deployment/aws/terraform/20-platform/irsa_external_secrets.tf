locals {
  external_secrets_subject = "system:serviceaccount:${var.external_secrets_namespace}:${var.external_secrets_service_account_name}"
}

data "aws_iam_policy_document" "external_secrets_assume_role" {
  count = var.create_external_secrets_irsa ? 1 : 0

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
      test     = "StringEquals"
      variable = "${local.oidc_hostpath}:sub"
      values   = [local.external_secrets_subject]
    }
  }
}

data "aws_iam_policy_document" "external_secrets_permissions" {
  count = var.create_external_secrets_irsa ? 1 : 0

  statement {
    sid    = "SecretsManagerRead"
    effect = "Allow"
    actions = [
      "secretsmanager:GetSecretValue",
      "secretsmanager:DescribeSecret",
      "secretsmanager:ListSecrets",
    ]
    resources = ["*"]
  }

  statement {
    sid    = "KmsDecrypt"
    effect = "Allow"
    actions = [
      "kms:Decrypt",
    ]
    resources = ["*"]
  }
}

resource "aws_iam_policy" "external_secrets" {
  count  = var.create_external_secrets_irsa ? 1 : 0
  name   = "${local.name}-external-secrets"
  policy = data.aws_iam_policy_document.external_secrets_permissions[0].json
  tags   = local.base_tags
}

resource "aws_iam_role" "external_secrets" {
  count              = var.create_external_secrets_irsa ? 1 : 0
  name               = "${local.name}-irsa-external-secrets"
  assume_role_policy = data.aws_iam_policy_document.external_secrets_assume_role[0].json
  tags               = local.base_tags
}

resource "aws_iam_role_policy_attachment" "external_secrets" {
  count      = var.create_external_secrets_irsa ? 1 : 0
  role       = aws_iam_role.external_secrets[0].name
  policy_arn = aws_iam_policy.external_secrets[0].arn
}

