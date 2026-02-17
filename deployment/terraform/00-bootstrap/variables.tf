variable "region" {
  description = "AWS region"
  type        = string
  default     = "eu-central-1"
}

variable "project" {
  description = "Project identifier used for naming"
  type        = string
  default     = "ittrap"
}

variable "env" {
  description = "Environment name (dev/stage/prod)"
  type        = string
  default     = "dev"
}

variable "state_bucket_name" {
  description = "Globally-unique S3 bucket name for Terraform state"
  type        = string
}

variable "lock_table_name" {
  description = "DynamoDB table name for Terraform state locks"
  type        = string
  default     = null
}

variable "tags" {
  description = "Tags applied to created resources"
  type        = map(string)
  default     = {}
}

