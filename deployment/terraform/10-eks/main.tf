provider "aws" {
  region = var.region
}

data "aws_availability_zones" "available" {}

locals {
  name = "${var.project}-${var.env}"

  az_count = var.learning_mode ? max(2, var.learning_az_count) : 2
  azs      = slice(data.aws_availability_zones.available.names, 0, local.az_count)

  base_tags = merge(
    {
      Project     = var.project
      Environment = var.env
      ManagedBy   = "terraform"
    },
    var.tags
  )
}

module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "~> 6.0"

  name = local.name
  cidr = var.vpc_cidr

  azs             = local.azs
  private_subnets = var.learning_mode ? [] : [for i, _ in local.azs : cidrsubnet(var.vpc_cidr, 4, i)]
  public_subnets  = [for i, _ in local.azs : cidrsubnet(var.vpc_cidr, 8, 100 + i)]

  enable_nat_gateway = var.learning_mode ? false : true
  single_nat_gateway = var.learning_mode ? false : true

  map_public_ip_on_launch = true

  public_subnet_tags = {
    "kubernetes.io/role/elb" = "1"
  }

  private_subnet_tags = {
    "kubernetes.io/role/internal-elb" = "1"
  }

  tags = local.base_tags
}

module "eks" {
  source  = "terraform-aws-modules/eks/aws"
  version = "~> 21.0"

  name    = local.name
  kubernetes_version = var.cluster_version

  vpc_id     = module.vpc.vpc_id
  subnet_ids = var.learning_mode ? module.vpc.public_subnets : module.vpc.private_subnets

  enable_cluster_creator_admin_permissions = true  

  eks_managed_node_groups = {
    default = {
      ami_type = "AL2023_x86_64_STANDARD"
      instance_types = var.learning_mode ? var.learning_instance_types : ["t3.medium"]
      min_size       = 1
      max_size       = var.learning_mode ? 1 : 3
      desired_size   = var.learning_mode ? var.learning_desired_size : 2

      iam_role_additional_policies = {
        AmazonEKSWorkerNodePolicy            = "arn:aws:iam::aws:policy/AmazonEKSWorkerNodePolicy"
        AmazonEKS_CNI_Policy                 = "arn:aws:iam::aws:policy/AmazonEKS_CNI_Policy"
        AmazonEC2ContainerRegistryReadOnly   = "arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryReadOnly"
        AmazonSSMManagedInstanceCore         = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
      }
    }
  }

  tags = local.base_tags
}

resource "aws_eks_addon" "vpc_cni" {
  cluster_name = local.name
  addon_name   = "vpc-cni"
}

resource "aws_eks_addon" "kube_proxy" {
  cluster_name = local.name
  addon_name   = "kube-proxy"
}

resource "aws_eks_addon" "coredns" {
  cluster_name = local.name
  addon_name   = "coredns"
}
