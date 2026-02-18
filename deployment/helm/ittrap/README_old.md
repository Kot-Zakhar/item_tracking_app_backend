# Helm chart: ittrap

This chart packages the Kubernetes manifests for your microservices.

## Naming

Resources use stable names (within the namespace) so service discovery stays predictable:

- `ittrap-user-service`
- `ittrap-location-service`
- `ittrap-management-service`

This matches the addresses hardcoded in the QueryService env vars (gRPC calls on port 81).

## AWS + IRSA

For AWS access (SQS/SNS), set:

- `global.irsa.enabled=true`
- `global.irsa.roleArn=<terraform output irsa_messaging_role_arn>`

The chart creates one ServiceAccount per service and annotates the ones with `useIrsa: true`.

## Config

Non-secret config is generated as ConfigMaps (mirrors `deployment/config-map.yaml`):
- SQS queue URLs
- SNS topic ARNs

Fill these from `deployment/terraform/20-platform` outputs (`sqs_queue_urls`, `sns_topic_arns`).

## Generating values from Terraform

To generate a values file from Terraform outputs:

```bash
./deployment/terraform/export-helm-values.sh
```

This creates `deployment/helm/ittrap/values-dev.generated.yaml` which you can:
- add to Argo CD `spec.source.helm.valueFiles`, or
- pass to Helm locally.

## Secrets

This chart does not create secrets by default. It expects Secrets like:
- `global-config` (jwt key, mediatr key, admin email/phone)
- `identity-service-config` (db conn string, passwordPepper)
- `user-service-config`, etc.

Recommended in AWS:
- store them in AWS Secrets Manager
- sync to K8s via External Secrets Operator (ESO)

## Render locally

```bash
helm template ittrap ./deployment/helm/ittrap -f ./deployment/helm/ittrap/values-dev.yaml
```
