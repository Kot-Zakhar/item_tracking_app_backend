# 00-bootstrap

Creates the remote state backend for Terraform:
- S3 bucket (stores `terraform.tfstate`)
- DynamoDB table (state locking)

Run this stage once per AWS account/region (or once per environment if you prefer).

## Usage

```bash
terraform init
terraform apply
```

Outputs show the bucket/table names you should plug into later stages (`backend.hcl`).

