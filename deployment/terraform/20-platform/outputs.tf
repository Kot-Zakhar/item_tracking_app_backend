output "ecr_repository_urls" {
  value = { for k, r in aws_ecr_repository.repos : k => r.repository_url }
}

output "sns_topic_arns" {
  value = { for k, t in aws_sns_topic.topics : k => t.arn }
}

output "sqs_queue_urls" {
  value = { for k, q in aws_sqs_queue.queues : k => q.url }
}

output "messaging_policy_arn" {
  value = aws_iam_policy.ittrap_messaging.arn
}

output "irsa_messaging_role_arn" {
  value = aws_iam_role.ittrap_irsa_messaging.arn
}

output "external_secrets_irsa_role_arn" {
  value = var.create_external_secrets_irsa ? aws_iam_role.external_secrets[0].arn : null
}
