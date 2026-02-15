variable "region" {
  description = "AWS region"
  type        = string
  default     = "eu-central-1"
}

variable "project" {
  type    = string
  default = "ittrap"
}

variable "env" {
  type    = string
  default = "dev"
}

variable "eks_state_bucket" {
  description = "S3 bucket name containing the 10-eks remote state"
  type        = string
}

variable "eks_state_key" {
  description = "Key/path for the 10-eks remote state"
  type        = string
  default     = "ittrap/dev/10-eks/terraform.tfstate"
}

variable "tags" {
  type    = map(string)
  default = {}
}

variable "k8s_namespace" {
  description = "Kubernetes namespace where service accounts live"
  type        = string
  default     = "ittrap"
}

variable "irsa_service_account_names" {
  description = "Kubernetes ServiceAccount names that should be allowed to assume the IRSA role"
  type        = list(string)
  default = [
    "ittrap-user-service",
    "ittrap-identity-service",
    "ittrap-query-service",
    "ittrap-inventory-service",
    "ittrap-location-service",
    "ittrap-management-service",
  ]
}

variable "create_rds_postgres" {
  description = "Create a shared RDS Postgres instance (starter setup)."
  type        = bool
  default     = false
}

variable "rds_instance_class" {
  description = "RDS instance class"
  type        = string
  default     = "db.t3.micro"
}

variable "rds_allocated_storage_gb" {
  description = "Allocated storage (GiB)"
  type        = number
  default     = 20
}

variable "rds_engine_version" {
  description = "Optional engine version (leave null to use AWS default for Postgres)."
  type        = string
  default     = null
}

variable "vpc_cidr_block_fallback" {
  description = "Fallback VPC CIDR block (only used if 10-eks doesn't output it)."
  type        = string
  default     = null
}

variable "create_external_secrets_irsa" {
  description = "Create an IRSA role for External Secrets Operator (ESO) to read AWS Secrets Manager."
  type        = bool
  default     = true
}

variable "external_secrets_namespace" {
  description = "Namespace where External Secrets Operator runs"
  type        = string
  default     = "external-secrets"
}

variable "external_secrets_service_account_name" {
  description = "ServiceAccount name used by External Secrets Operator"
  type        = string
  default     = "external-secrets"
}
