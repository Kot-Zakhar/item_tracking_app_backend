provider "aws" {
  region = var.region
}

locals {
  lock_table = coalesce(var.lock_table_name, "${var.project}-${var.env}-tf-locks")

  base_tags = merge(
    {
      Project     = var.project
      Environment = var.env
      ManagedBy   = "terraform"
    },
    var.tags
  )
}

resource "aws_s3_bucket" "tf_state" {
  bucket = var.state_bucket_name
  tags   = local.base_tags
}

resource "aws_s3_bucket_versioning" "tf_state" {
  bucket = aws_s3_bucket.tf_state.id
  versioning_configuration {
    status = "Enabled"
  }
}

resource "aws_s3_bucket_server_side_encryption_configuration" "tf_state" {
  bucket = aws_s3_bucket.tf_state.id
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_public_access_block" "tf_state" {
  bucket                  = aws_s3_bucket.tf_state.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_dynamodb_table" "tf_locks" {
  name         = local.lock_table
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "LockID"

  attribute {
    name = "LockID"
    type = "S"
  }

  tags = local.base_tags
}

