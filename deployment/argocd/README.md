# Argo CD setup for this repo (Helm)

This directory contains Argo CD `Application` manifests for deploying the Helm chart in `deployment/helm/ittrap`.

## 1) Install Argo CD (one-time)

Example using Helm:

```bash
kubectl create namespace argocd
helm repo add argo https://argoproj.github.io/argo-helm
helm repo update
helm install argocd argo/argo-cd -n argocd
```

## 1.5) Platform apps (recommended)

This repo includes Argo CD apps for common EKS controllers:

- `deployment/argocd/apps/platform-external-secrets.yaml` (External Secrets Operator)
- `deployment/argocd/apps/platform-external-secrets-config.yaml` (ClusterSecretStore for AWS)
- `deployment/argocd/apps/platform-aws-load-balancer-controller.yaml` (ALB/NLB Ingress support)

Both include placeholders you must fill (IRSA role arns, cluster name, VPC id).

## 2) Create a root "app-of-apps" (recommended)

Edit `deployment/argocd/root-app.yaml`:
- set `spec.source.repoURL` to your git repo URL
- set `spec.source.targetRevision` (e.g. `main`)

Then apply:

```bash
kubectl apply -n argocd -f deployment/argocd/root-app.yaml
```

Argo CD will then create/sync the child apps from `deployment/argocd/apps/`.

## 3) Configure values for AWS

Update `deployment/helm/ittrap/values-dev.yaml` with outputs from Terraform:

- `global.irsa.roleArn` = `deployment/terraform/20-platform` output `irsa_messaging_role_arn`
- `global.config.sqs.*` = `deployment/terraform/20-platform` output `sqs_queue_urls`
- `global.config.sns.*` = `deployment/terraform/20-platform` output `sns_topic_arns`

## 4) Secrets

The Helm chart expects Kubernetes Secrets to exist (e.g. `global-config`, `identity-service-config`, etc).

For AWS/EKS, the usual approach is:
- install External Secrets Operator (ESO)
- create a `ClusterSecretStore` for AWS Secrets Manager using IRSA
- define `ExternalSecret` resources to materialize K8s secrets per service

This repo includes an Argo CD app that can install ESO (`deployment/argocd/apps/platform-external-secrets.yaml`).

For ESO on EKS, you typically:

1) Create an IRSA role for ESO (this repo can create one via Terraform):
   - `deployment/terraform/20-platform` output `external_secrets_irsa_role_arn`
2) Put that ARN into `deployment/argocd/apps/platform-external-secrets.yaml` (values block).
3) Create a `ClusterSecretStore` that uses `auth.jwt.serviceAccountRef` pointing at ESO’s service account.
