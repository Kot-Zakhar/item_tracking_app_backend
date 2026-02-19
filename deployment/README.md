# Local Kubernetes with ArgoCD Deployment

This directory demonstrates a **GitOps** approach to deploying the Item Tracking App using ArgoCD on a local Kubernetes cluster. This setup represents modern cloud-native deployment practices, where the desired state of your infrastructure and applications is declaratively defined in Git, and ArgoCD continuously ensures the cluster matches that state.

## What is GitOps?

GitOps is a modern approach to continuous deployment where:

- **Git is the single source of truth** - All infrastructure and application configurations live in Git
- **Declarative configuration** - You define what you want, not how to achieve it
- **Automated synchronization** - ArgoCD monitors Git and automatically applies changes to the cluster
- **Audit trail** - Every change is tracked in Git history
- **Easy rollbacks** - Just revert a Git commit to roll back

## Overview

This deployment uses:

- **Helm Charts** - Package Kubernetes manifests with templating and variables
- **ArgoCD** - GitOps continuous delivery tool for Kubernetes
- **App of Apps Pattern** - A root ArgoCD application that manages child applications
- **HashiCorp Vault** - Secret management (optional but recommended)
- **External Secrets Operator** - Syncs secrets from Vault to Kubernetes
- **LocalStack** - AWS services emulation for local development
- **Mailpit** - Modern email testing tool

## Architecture

### Directory Structure

```
local-kubernetes-argocd/
├── README.md                    # This file
├── argocd/                      # ArgoCD application definitions
│   ├── README.md               # ArgoCD-specific documentation
│   ├── root-app.yaml           # Root "App of Apps"
│   ├── apps/                   # Child application manifests
│   │   ├── vault-app.yaml      # Deploys HashiCorp Vault
│   │   ├── external-secrets-app.yaml  # Deploys External Secrets Operator
│   │   ├── localstack.yaml     # Deploys LocalStack (AWS emulation)
│   │   ├── mailpit.yaml        # Deploys Mailpit (email testing)
│   │   └── ittrap-dev.yaml     # Main application deployment
│   └── manifests/              # Additional Kubernetes manifests
│       └── mailpit/            # Mailpit custom manifests
└── helm/                        # Helm charts
    └── ittrap/                 # Main application chart
        ├── Chart.yaml          # Chart metadata
        ├── values.yaml         # Default values
        ├── values-dev.yaml     # Development/local overrides
        └── templates/          # Kubernetes manifest templates
```

### Component Flow

1. **Vault** (sync-wave: 0) - Deploys first, stores secrets
2. **External Secrets** (sync-wave: 1) - Syncs secrets from Vault to K8s
3. **LocalStack** (sync-wave: 8) - AWS services emulation
4. **Mailpit** (sync-wave: 9) - Email testing
5. **ItTrAp Application** (sync-wave: 10) - Main microservices

The sync-wave annotations control deployment order, ensuring dependencies are ready before dependent services start.

## Prerequisites

### Required Tools

1. **Kubernetes Cluster** - Choose one:
   - **Minikube** (Recommended)
     ```bash
     # Start with sufficient resources
     minikube start --cpus=4 --memory=8192 --disk-size=20g
     ```
   - **Kind**
     ```bash
     kind create cluster --name ittrap
     ```

2. **kubectl** - Kubernetes CLI
   ```bash
   kubectl version --client
   ```

3. **Helm** - Kubernetes package manager
   ```bash
   # Install Helm 3
   curl https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 | bash
   
   # Verify
   helm version
   ```

4. **ArgoCD CLI** (Optional but recommended)
   ```bash
   # Linux
   curl -sSL -o /tmp/argocd-linux-amd64 https://github.com/argoproj/argo-cd/releases/latest/download/argocd-linux-amd64
   sudo install -m 555 /tmp/argocd-linux-amd64 /usr/local/bin/argocd
   
   # Verify
   argocd version --client
   ```

### System Requirements

- **CPU**: 4+ cores
- **RAM**: 8GB minimum, 12GB recommended
- **Disk**: 20GB free space
- **Kubernetes**: v1.24 or higher
- **Helm**: v3.8 or higher

### Verify Setup

```bash
# Check cluster is running
kubectl cluster-info
kubectl get nodes

# Check Helm is working
helm list -A
```

## Quick Start

### Step 1: Install ArgoCD

Install ArgoCD in your cluster:

```bash
# Create ArgoCD namespace
kubectl create namespace argocd

# Install ArgoCD using Helm
helm repo add argo https://argoproj.github.io/argo-helm
helm repo update
helm install argocd argo/argo-cd -n argocd

# Wait for ArgoCD to be ready
kubectl wait --for=condition=available --timeout=300s \
  pod -l app.kubernetes.io/name=argocd-application-controller -n argocd
```

### Step 2: Access ArgoCD UI

**Method 1: Port Forward (Recommended for local)**

```bash
# Forward ArgoCD server port
kubectl port-forward svc/argocd-server -n argocd 8080:443

# Access UI at: https://localhost:8080
# (Accept the self-signed certificate warning)
```

**Method 2: NodePort (for Minikube)**

```bash
# Patch service to NodePort
kubectl patch svc argocd-server -n argocd -p '{"spec": {"type": "NodePort"}}'

# Get the URL
minikube service argocd-server -n argocd --url
```

### Step 3: Login to ArgoCD

**Get Initial Admin Password:**

```bash
# Retrieve password
kubectl -n argocd get secret argocd-initial-admin-secret \
  -o jsonpath="{.data.password}" | base64 -d && echo
```

**Login via UI:**
- Username: `admin`
- Password: (from command above)

**Login via CLI:**

```bash
# Get server address
ARGOCD_SERVER=$(kubectl get svc argocd-server -n argocd -o jsonpath='{.status.loadBalancer.ingress[0].hostname}')

# Or for port-forward
ARGOCD_SERVER=localhost:8080

# Login
argocd login $ARGOCD_SERVER --username admin --insecure
# Enter password when prompted
```

**Change Admin Password (Recommended):**

```bash
argocd account update-password
```

### Step 4: Configure Secrets

Before deploying, you need to set up secrets.

#### Vault + External Secrets

See the [Vault Configuration](#vault-configuration) section below.

### Step 5: Deploy with ArgoCD

**Deploy the Root App (App of Apps):**

```bash
# Apply the root application
kubectl apply -f deployment/argocd/root-app-local.yaml
```

This single command will:
1. Create the root ArgoCD application
2. ArgoCD discovers child applications in `argocd/apps/`
3. ArgoCD deploys them in order based on sync-wave
4. ArgoCD continuously monitors and syncs with Git

**Watch Deployment Progress:**

```bash
# Watch ArgoCD applications
kubectl get applications -n argocd -w

# Watch pods being created
kubectl get pods -n ittrap -w

# Via ArgoCD CLI
argocd app list
argocd app get ittrap-dev
```

**Via ArgoCD UI:**

Open https://localhost:8080 and watch the beautiful visualization of your applications syncing!

### Step 6: Verify Deployment

```bash
# Check all applications are healthy
argocd app list

# Check pods in ittrap namespace
kubectl get pods -n ittrap

# Check all pods across namespaces
kubectl get pods --all-namespaces | grep -E '(vault|localstack|mailpit|ittrap)'

# Check services
kubectl get svc -n ittrap
```

All applications should show:
- **Status**: Synced
- **Health**: Healthy

### Step 7: Access the Application

**API Gateway:**

```bash
# For Minikube
minikube service ittrap-api-gateway-nodeport -n ittrap --url

# For port-forward
kubectl port-forward -n ittrap svc/ittrap-api-gateway-nodeport 8081:80

# Test
curl http://localhost:8081/health
```

**Mailpit (Email UI):**

```bash
kubectl port-forward -n mailpit svc/mailpit 8025:8025

# Access at: http://localhost:8025
```

**Vault UI:**

```bash
kubectl port-forward -n vault svc/vault 8200:8200

# Access at: http://localhost:8200
```


**LocalStack (AWS Services):**

```bash
kubectl port-forward -n localstack svc/localstack 4566:4566

# Test
aws --endpoint-url=http://localhost:4566 sqs list-queues
```

## Understanding the Setup

### App of Apps Pattern

The root application ([root-app.yaml](argocd/root-app.yaml)) manages child applications:

```yaml
apiVersion: argoproj.io/v1alpha1
kind: Application
metadata:
  name: ittrap-root
spec:
  source:
    path: deployment/argocd/apps  # Points to child apps
    directory:
      recurse: true               # Discovers all YAML files
  syncPolicy:
    automated:
      prune: true                 # Deletes resources not in Git
      selfHeal: true              # Auto-corrects manual changes
```

Benefits:
- **Single command** to deploy everything
- **Automatic discovery** of new applications
- **Centralized management** of all components
- **Consistent policies** across applications

### Sync Waves

Applications deploy in order using sync-wave annotations:

| Wave | Application | Purpose |
|------|-------------|---------|
| 0 | Vault | Secret storage |
| 1 | External Secrets | Secret synchronization |
| 8 | LocalStack | AWS services |
| 9 | Mailpit | Email testing |
| 10 | ItTrAp | Main application |

This ensures dependencies are ready before dependents start.

### Helm Values Hierarchy

The Helm chart uses a layered values approach:

1. **values.yaml** - Base/production values
2. **values-dev.yaml** - Local development overrides

For local deployment, [values-dev.yaml](helm/ittrap/values-dev.yaml) overrides production settings:
- Points to LocalStack instead of real AWS
- Uses localhost domain
- Enables local ingress configuration
- Adjusts resource limits for local clusters

### GitOps Workflow

1. **Make changes** - Edit files in this directory
2. **Commit to Git** - Push changes to your repository
3. **ArgoCD syncs** - Automatically applies changes to cluster
4. **Observe** - Watch in ArgoCD UI or CLI

No manual `kubectl apply` needed after initial setup!

## Helm Chart Structure

The main application is packaged as a Helm chart in [helm/ittrap/](helm/ittrap/).

### Key Templates

- [namespace.yaml](helm/ittrap/templates/namespace.yaml) - Creates `ittrap` namespace
- [configmaps.yaml](helm/ittrap/templates/configmaps.yaml) - Non-sensitive configuration
- [secret-store.yaml](helm/ittrap/templates/secret-store.yaml) - External Secrets config
- [external-secrets.yaml](helm/ittrap/templates/external-secrets.yaml) - Secret definitions
- [api-gateway.yaml](helm/ittrap/templates/api-gateway.yaml) - API Gateway deployment
- [*-service.yaml](helm/ittrap/templates/) - Microservice deployments

### Testing Helm Templates Locally

```bash
# Render templates with default values
helm template ittrap ./helm/ittrap

# Render with dev values
helm template ittrap ./helm/ittrap -f ./helm/ittrap/values-dev.yaml

# Check for errors
helm lint ./helm/ittrap

# Dry-run install
helm install ittrap ./helm/ittrap --dry-run --debug -n ittrap
```

## Vault Configuration

For production-like secret management:

### Step 1: Unseal Vault

Vault starts sealed on every pod restart. Run the unseal script — it will initialize Vault on first run (saving keys and the root token to `scripts/vault-init-keys.json`) and unseal it on subsequent runs:

```bash
./scripts/unseal-vault.sh
```

> ⚠️ `vault-init-keys.json` contains your root token and unseal keys. It is `.gitignore`d — keep it safe and never commit it.

### Step 2: Store Secrets in Vault

Copy the env template and fill in your values:

```bash
cp scripts/fill-vault.env.example scripts/fill-vault.env
# edit scripts/fill-vault.env
```

Then run the script — it reads the root token from `vault-init-keys.json` automatically:

```bash
./scripts/fill-vault.sh
```

This script will:
- Authenticate to Vault using the root token from `vault-init-keys.json`
- Enable the KV v2 secrets engine
- Populate all secrets at their respective paths (`kv/ittrap/*`, `kv/postgres`, `kv/mongo`)
- Create scoped policies for each namespace (`ittrap-policy`, `postgres-policy`, `mongo-policy`)
- Enable Kubernetes auth and register roles for each namespace (`ittrap-eso`, `postgres-eso`, `mongo-eso`)

### Step 3: Configure External Secrets

The External Secrets Operator will automatically sync secrets from Vault to Kubernetes based on the `ExternalSecret` resources in each namespace (`ittrap`, `postgres`, `mongo`).

## Common Operations

### Viewing Application Status

```bash
# List all applications
argocd app list

# Get detailed info
argocd app get ittrap-dev

# View sync status
argocd app sync-status ittrap-dev

# View application tree
argocd app resources ittrap-dev
```

### Syncing Applications

```bash
# Sync a single application
argocd app sync ittrap-dev

# Sync all applications
argocd app sync ittrap-root

# Force hard sync (override manual changes)
argocd app sync ittrap-dev --force

# Sync and prune
argocd app sync ittrap-dev --prune
```

### Viewing Logs

```bash
# Via kubectl
kubectl logs -n ittrap -l app=ittrap-api-gateway -f

# Via ArgoCD UI - click on pod in the UI

# Via ArgoCD CLI
argocd app logs ittrap-dev
```

### Updating Configuration

**Method 1: Edit values file (GitOps way)**

```bash
# Edit values-dev.yaml
vim deployment/local-kubernetes-argocd/helm/ittrap/values-dev.yaml

# Commit and push
git add deployment/local-kubernetes-argocd/helm/ittrap/values-dev.yaml
git commit -m "Update configuration"
git push

# ArgoCD auto-syncs (if enabled) or manual sync
argocd app sync ittrap-dev
```

**Method 2: Override via ArgoCD parameters**

```bash
# Override a specific value
argocd app set ittrap-dev \
  --helm-set services.apiGateway.replicas=3

# Sync
argocd app sync ittrap-dev
```

### Scaling Services

**Via Helm values:**

Edit [values-dev.yaml](helm/ittrap/values-dev.yaml):

```yaml
services:
  apiGateway:
    replicas: 3  # Change from 1 to 3
```

Commit, push, and sync.

**Via kubectl (temporary):**

```bash
kubectl scale deployment ittrap-api-gateway-deployment -n ittrap --replicas=3
```

Note: ArgoCD will revert this if self-heal is enabled!

### Rolling Back

**Via Git:**

```bash
# Revert the commit
git revert <commit-hash>
git push

# ArgoCD syncs automatically
```

**Via ArgoCD:**

```bash
# Rollback to previous version
argocd app rollback ittrap-dev

# Rollback to specific revision
argocd app rollback ittrap-dev <revision-number>
```

### Debugging

```bash
# View application events
argocd app events ittrap-dev

# Get manifest that would be applied
argocd app manifests ittrap-dev

# Compare live vs desired state
argocd app diff ittrap-dev

# View sync result
argocd app get ittrap-dev --show-operation
```

## Advanced Topics

### Multi-Environment Setup

Create separate value files for each environment:

```bash
helm/ittrap/
├── values.yaml          # Production
├── values-dev.yaml      # Development
├── values-staging.yaml  # Staging
└── values-test.yaml     # Testing
```

Create corresponding ArgoCD apps:

```yaml
# ittrap-staging.yaml
spec:
  source:
    helm:
      valueFiles:
        - values.yaml
        - values-staging.yaml
```

## Cleanup

### Remove All Applications

```bash
# Delete root app (cascades to children)
argocd app delete ittrap-root --cascade

# Or via kubectl
kubectl delete application ittrap-root -n argocd

# Verify deletion
kubectl get applications -n argocd
```

### Remove ArgoCD

```bash
# Delete ArgoCD installation
helm uninstall argocd -n argocd

# Delete namespace
kubectl delete namespace argocd
```

### Complete Cleanup

```bash
# Delete all resources
kubectl delete namespace ittrap
kubectl delete namespace vault
kubectl delete namespace external-secrets
kubectl delete namespace localstack
kubectl delete namespace mailpit
kubectl delete namespace argocd

# Stop cluster
minikube stop
minikube delete

# Or for Kind
kind delete cluster --name ittrap
```

---

**Note**: This configuration is optimized for learning and local development. For production deployments, implement additional security measures, proper secret management, monitoring, backup strategies, and network policies.