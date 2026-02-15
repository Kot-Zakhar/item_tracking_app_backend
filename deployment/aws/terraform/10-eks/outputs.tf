output "cluster_name" {
  value = module.eks.cluster_name
}

output "cluster_endpoint" {
  value = module.eks.cluster_endpoint
}

output "cluster_oidc_issuer_url" {
  value = module.eks.cluster_oidc_issuer_url
}

output "oidc_provider_arn" {
  value = try(module.eks.oidc_provider_arn, null)
}

output "vpc_id" {
  value = module.vpc.vpc_id
}

output "vpc_cidr_block" {
  value = try(module.vpc.vpc_cidr_block, null)
}

output "private_subnet_ids" {
  value = module.vpc.private_subnets
}

output "node_security_group_id" {
  value = try(module.eks.node_security_group_id, null)
}

output "region" {
  value = var.region
}

output "configure_kubectl" {
  value = "aws eks update-kubeconfig --region ${var.region} --name ${module.eks.cluster_name}"
}
