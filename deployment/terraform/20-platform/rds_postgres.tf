locals {
  vpc_id                 = data.terraform_remote_state.eks.outputs.vpc_id
  vpc_cidr_block         = coalesce(data.terraform_remote_state.eks.outputs.vpc_cidr_block, var.vpc_cidr_block_fallback)
  private_subnet_ids     = data.terraform_remote_state.eks.outputs.private_subnet_ids
  node_security_group_id = try(data.terraform_remote_state.eks.outputs.node_security_group_id, null)
}

resource "random_password" "rds_master" {
  count   = var.create_rds_postgres ? 1 : 0
  length  = 32
  special = true
}

resource "aws_secretsmanager_secret" "rds_master" {
  count = var.create_rds_postgres ? 1 : 0
  name  = "${local.name}/rds/postgres/master"
  tags  = local.base_tags
}

resource "aws_secretsmanager_secret_version" "rds_master" {
  count     = var.create_rds_postgres ? 1 : 0
  secret_id = aws_secretsmanager_secret.rds_master[0].id
  secret_string = jsonencode({
    username = "ittrap_admin"
    password = random_password.rds_master[0].result
  })
}

resource "aws_db_subnet_group" "ittrap" {
  count      = var.create_rds_postgres ? 1 : 0
  name       = "${local.name}-rds-subnets"
  subnet_ids = local.private_subnet_ids
  tags       = local.base_tags
}

resource "aws_security_group" "rds" {
  count       = var.create_rds_postgres ? 1 : 0
  name        = "${local.name}-rds"
  description = "RDS access from EKS nodes"
  vpc_id      = local.vpc_id
  tags        = local.base_tags
}

resource "aws_security_group_rule" "rds_from_nodes" {
  count                    = var.create_rds_postgres && local.node_security_group_id != null ? 1 : 0
  type                     = "ingress"
  from_port                = 5432
  to_port                  = 5432
  protocol                 = "tcp"
  security_group_id        = aws_security_group.rds[0].id
  source_security_group_id = local.node_security_group_id
  description              = "Allow Postgres from EKS nodes"
}

resource "aws_security_group_rule" "rds_from_vpc_cidr" {
  count             = var.create_rds_postgres && local.node_security_group_id == null && local.vpc_cidr_block != null ? 1 : 0
  type              = "ingress"
  from_port         = 5432
  to_port           = 5432
  protocol          = "tcp"
  security_group_id = aws_security_group.rds[0].id
  cidr_blocks       = [local.vpc_cidr_block]
  description       = "Allow Postgres from VPC CIDR (fallback if node SG output is unavailable)"
}

resource "aws_security_group_rule" "rds_egress" {
  count             = var.create_rds_postgres ? 1 : 0
  type              = "egress"
  from_port         = 0
  to_port           = 0
  protocol          = "-1"
  security_group_id = aws_security_group.rds[0].id
  cidr_blocks       = ["0.0.0.0/0"]
}

resource "aws_db_instance" "postgres" {
  count = var.create_rds_postgres ? 1 : 0

  identifier = "${local.name}-postgres"

  engine         = "postgres"
  engine_version = var.rds_engine_version

  instance_class    = var.rds_instance_class
  allocated_storage = var.rds_allocated_storage_gb

  db_name  = "ittrap"
  username = "ittrap_admin"
  password = random_password.rds_master[0].result

  db_subnet_group_name   = aws_db_subnet_group.ittrap[0].name
  vpc_security_group_ids = [aws_security_group.rds[0].id]

  multi_az            = false
  publicly_accessible = false
  storage_encrypted   = true

  backup_retention_period = 3
  skip_final_snapshot     = true

  tags = local.base_tags
}

output "rds_postgres_endpoint" {
  value       = var.create_rds_postgres ? aws_db_instance.postgres[0].address : null
  description = "Hostname for Postgres. Prefer consuming credentials from Secrets Manager."
}

output "rds_master_secret_arn" {
  value       = var.create_rds_postgres ? aws_secretsmanager_secret.rds_master[0].arn : null
  description = "Secrets Manager secret containing JSON {username,password}."
}
