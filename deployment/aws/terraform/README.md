# Terraform on AWS for this repo (EKS + managed DBs)

This folder is a learning-friendly, staged Terraform setup to deploy the infrastructure for your microservices onto **AWS + EKS**, while moving stateful components to **managed services**.

Your current `deployment/` manifests are Kubernetes-first (Deployments/Services, NodePorts, in-cluster Postgres/Mongo, Localstack for SNS/SQS). Terraform will provision the AWS equivalents so your K8s deployment can target real AWS.

## What Terraform should manage vs Kubernetes

Terraform (cloud infrastructure):
- EKS cluster (control plane + node groups)
- VPC/subnets/security groups
- Container registry (ECR)
- Messaging (SNS topics, SQS queues)
- Managed Postgres (RDS)
- Managed Mongo option (DocumentDB or MongoDB Atlas)
- IAM/IRSA (pods access AWS without static keys)
- Secrets storage (AWS Secrets Manager / SSM Parameter Store)

Kubernetes (application runtime):
- Deployments/Services/Ingress
- ConfigMaps (non-secret config)
- Autoscaling, rollouts
- Secret syncing (via External Secrets Operator) or direct injection at deploy time

## Stages

Terraform is split into stages so you can learn and debug in layers:

1) `00-bootstrap/` – creates Terraform remote state backend (S3 + DynamoDB lock)
2) `10-eks/` – creates VPC + EKS cluster and outputs cluster details
3) `20-platform/` – creates ECR, SNS/SQS, IAM roles for service accounts (IRSA), and optional RDS

## About “free-tier-ish”

EKS is great for learning Kubernetes on AWS, but it is not truly “free tier”:
- EKS control plane is billed per cluster-hour
- VPC NAT Gateway is billed per hour + per GB (this repo’s `10-eks` defaults disable NAT in `learning_mode` to reduce cost)

For a rough monthly estimate for an always-on demo, see `deployment/terraform/COSTS.md`.

## Prereqs (local machine)

- AWS account + IAM user/role with permissions to create VPC/EKS/IAM/S3/DynamoDB (broad perms at first; tighten later)
- `aws` CLI configured (`aws configure` or SSO), and a profile you’ll use consistently
- `terraform` (>= 1.6 recommended)
- `kubectl`

## Important security note

`deployment/secrets.yaml` currently contains base64-encoded secrets checked into git. For real AWS, prefer:
- AWS Secrets Manager (store secrets)
- External Secrets Operator (sync secrets into Kubernetes)
- IRSA (avoid AWS access keys in Kubernetes altogether)

## Suggested first run (happy path)

### 0) Pick names

- `project`: `ittrap`
- `env`: `dev`
- `region`: `eu-central-1` (or any)
- Choose a globally-unique S3 bucket name for Terraform state (S3 bucket names are global).

### 1) Create the remote state backend

From `deployment/terraform/00-bootstrap/`:

```bash
terraform init
terraform apply
```

### 2) Create EKS

From `deployment/terraform/10-eks/`:

1. Copy `backend.hcl.example` to `backend.hcl` and fill in your values.
2. Then:

```bash
terraform init -backend-config=backend.hcl
terraform apply
```

### 3) Create platform resources (ECR + SNS/SQS + IAM + optional RDS)

From `deployment/terraform/20-platform/`:

1. Copy `backend.hcl.example` to `backend.hcl` and fill in your values.
2. Then:

```bash
terraform init -backend-config=backend.hcl
terraform apply
```

## Managed DBs: what to migrate first

You have multiple Postgres instances today (one per service). For learning + cost control, migrate one at a time:
- start with IdentityService (auth is usually the most “infra-integrated”)
- then UserService, then others

For Inventory’s Mongo:
- AWS DocumentDB is “Mongo-compatible” but not 100% Mongo feature parity.
- MongoDB Atlas is often the smoothest “managed Mongo” experience (still works great with AWS/EKS).

## Connecting Kubernetes to AWS resources (high level)

Once `20-platform/` is applied:

- Replace Localstack config:
  - remove `AWS__ServiceURL` overrides (that’s only for Localstack)
  - use real AWS endpoints (default SDK behavior) and IRSA
- Replace SNS/SQS config values in K8s ConfigMaps with Terraform outputs (real topic ARNs + queue URLs)
- Replace Postgres StatefulSets with RDS connection strings (via Secrets Manager + external-secrets)

## Terraform → Helm values (wiring)

Terraform produces AWS identifiers (SNS topic ARNs, SQS queue URLs, IRSA role ARN). Helm needs them as values to render the Kubernetes manifests.

To avoid copy/paste, generate a Helm values file from Terraform outputs:

```bash
./deployment/terraform/export-helm-values.sh
```

This writes `deployment/helm/ittrap/values-dev.generated.yaml` (gitignored). Add it to Argo CD’s Helm `valueFiles` or use it with `helm template`.

## External Secrets Operator (recommended)

`20-platform/` can create an IRSA role for External Secrets Operator to read AWS Secrets Manager:
- Terraform output: `external_secrets_irsa_role_arn`

You can wire that into Argo CD via `deployment/argocd/apps/platform-external-secrets.yaml`.

## Next steps (recommended learning order)

1) Get EKS up and connect to it (`kubectl get nodes`)
2) Push images to ECR (or keep Docker Hub for now, but ECR is better on AWS)
3) Install an Ingress Controller (AWS Load Balancer Controller) and expose only the API gateway publicly
4) Migrate one service end-to-end:
   - e.g. IdentityService + its DB + SQS queue
   - then repeat for others
