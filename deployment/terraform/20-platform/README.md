# 20-platform

Creates shared AWS resources used by your services:
- ECR repositories for images
- SNS topics + SQS queues (replace Localstack)
- IRSA role for pods (so pods can use AWS without access keys)
- Optional: RDS Postgres instance (starter setup)

This stage expects `10-eks/` to already exist and uses remote state to read EKS outputs.

## Backend config

Copy `backend.hcl.example` → `backend.hcl` and fill it in.

## Usage

```bash
terraform init -backend-config=backend.hcl
terraform apply
```

## Inputs you must set

- `eks_state_bucket`: your remote state bucket (from `00-bootstrap/` output)
- `eks_state_key`: defaults to `ittrap/dev/10-eks/terraform.tfstate` (change if you changed env/project)

## How this maps to your current K8s manifests

Your `deployment/config-map.yaml` currently points to:
- `outbound-sns-topic-arn` values like `arn:aws:sns:...:user-events`
- `sqs-url` values like `http://ittrap-localstack-service:4566/.../auth-queue`

After this stage:
- update ConfigMaps to use the real Terraform outputs:
  - topic ARNs (SNS)
  - queue URLs (SQS)
- remove `AWS__ServiceURL` and `AWS_ACCESS_KEY_ID/AWS_SECRET_ACCESS_KEY` from your Deployments
  - IRSA provides credentials automatically to the pod

## IRSA (what you do in Kubernetes)

This stage outputs `irsa_messaging_role_arn`. Create ServiceAccounts and annotate them with that role:

```yaml
apiVersion: v1
kind: ServiceAccount
metadata:
  name: ittrap-query-service
  namespace: ittrap
  annotations:
    eks.amazonaws.com/role-arn: arn:aws:iam::123456789012:role/ittrap-dev-irsa-messaging
```

Then reference that ServiceAccount from your Deployment (`spec.template.spec.serviceAccountName`).

This stage also can create an IRSA role for External Secrets Operator (ESO) to read from AWS Secrets Manager:
- Terraform output: `external_secrets_irsa_role_arn`

## Optional RDS Postgres

Set `create_rds_postgres=true` to create a single shared Postgres instance plus a Secrets Manager secret for the master credentials.

This is intentionally a “starter” setup to get you working quickly; you can evolve it to:
- one RDS instance per microservice, or
- one instance with separate DB users/schemas, plus network hardening.

If your `10-eks/` module output doesn’t expose a node security group id, this stage falls back to allowing Postgres from the VPC CIDR.
If needed, set `vpc_cidr_block_fallback` to match your VPC CIDR (default in this repo is `10.20.0.0/16`).
