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

variable "learning_mode" {
  description = "Cost-optimized learning mode: public subnets only, no NAT Gateway, smaller node group."
  type        = bool
  default     = false
}

variable "learning_az_count" {
  description = "Number of AZs to use in learning mode (EKS requires subnets in at least 2 AZs)."
  type        = number
  default     = 2
}

variable "learning_instance_types" {
  description = "EKS node instance types in learning mode."
  type        = list(string)
  default     = ["t3.micro"]
}

variable "learning_desired_size" {
  description = "Desired node count in learning mode."
  type        = number
  default     = 1
}

variable "cluster_version" {
  description = "EKS Kubernetes version"
  type        = string
  default     = "1.34"
}

variable "vpc_cidr" {
  type    = string
  default = "10.20.0.0/16"
}

variable "tags" {
  type    = map(string)
  default = {}
}
