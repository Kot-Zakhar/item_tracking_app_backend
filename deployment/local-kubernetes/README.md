# Local Kubernetes Deployment

This directory contains Kubernetes manifests to deploy the entire Item Tracking App microservices architecture on a local Kubernetes cluster.

## Overview

The local Kubernetes deployment demonstrates a microservices architecture running in a containerized orchestration environment. This approach allows you to:

- **Understand Kubernetes concepts** - Learn how microservices run in production-like environments
- **Test service scaling** - Experiment with replicas and resource limits
- **Practice DevOps workflows** - Deploy, update, and rollback services
- **Validate service mesh patterns** - Test service-to-service communication
- **Develop cloud-native skills** - Work with ConfigMaps, Secrets, StatefulSets, and more

## Architecture Components

### Microservices (Deployments)
- **API Gateway** - Entry point exposed via NodePort on port 30080
- **Query Service** - Read-optimized aggregation service
- **Identity Service** - Authentication and user management
- **Inventory Service** - Item and inventory management
- **Management Service** - Administrative operations
- **Location Service** - Location hierarchy management
- **User Service** - User profile service
- **Email Service** - Email notifications

### Infrastructure (StatefulSets)
- **PostgreSQL** - Shared database with persistent storage
- **MongoDB** - Document store for inventory data
- **LocalStack** - AWS services emulation (SNS/SQS)
- **MailHog** - Email testing service

### Kubernetes Resources
- **Namespace**: `ittrap` - Isolated environment for all resources
- **ConfigMaps**: Non-sensitive configuration (URLs, queues, topics)
- **Secrets**: Sensitive data (database credentials, JWT keys)
- **Services**: ClusterIP for internal communication, NodePort for external access
- **StatefulSets**: Stateful components with persistent volumes
- **Deployments**: Stateless application services

## Prerequisites

### Required Tools

1. **Kubernetes Cluster** - Choose one:
   - **Minikube** (Recommended for beginners)
     ```bash
     # Install on Linux
     curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
     sudo install minikube-linux-amd64 /usr/local/bin/minikube
     ```
   - **Kind** (Kubernetes in Docker)
     ```bash
     curl -Lo ./kind https://kind.sigs.k8s.io/dl/v0.20.0/kind-linux-amd64
     chmod +x ./kind
     sudo mv ./kind /usr/local/bin/kind
     ```
   - **Docker Desktop** (macOS/Windows) - Enable Kubernetes in settings
   - **K3s/K3d** (Lightweight Kubernetes)

2. **kubectl** - Kubernetes CLI
   ```bash
   curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl"
   sudo install -o root -g root -m 0755 kubectl /usr/local/bin/kubectl
   ```

3. **Docker** - For building images
   ```bash
   docker --version  # Should be 20.10 or higher
   ```

### Verify Installation

```bash
# Check kubectl
kubectl version --client

# Check cluster is running (start if needed)
minikube start  # For Minikube
# OR
kind create cluster  # For Kind

# Verify cluster access
kubectl cluster-info
kubectl get nodes
```

### System Requirements

- **CPU**: 4+ cores recommended
- **RAM**: 8GB minimum, 16GB recommended
- **Disk**: 20GB free space
- **Kubernetes**: v1.24 or higher

## Configuration

### 1. Prepare Secrets

The [secrets.yaml](secrets.yaml) file contains placeholders for sensitive data. You need to replace them with base64-encoded values.

#### Generate and Encode Your Secrets

```bash
# JWT Private Key (at least 32 characters)
echo -n "your-secret-jwt-key-minimum-32-characters" | base64

# MediatR License Key (if you have one, otherwise use placeholder)
echo -n "your-mediatr-license-key" | base64

# Admin credentials
echo -n "admin@example.com" | base64
echo -n "+1234567890" | base64

# AWS credentials (for LocalStack)
echo -n "test" | base64

# Password pepper
echo -n "your-random-pepper-string" | base64

# Database credentials
echo -n "admin" | base64
echo -n "password@1234" | base64
echo -n "item_tracking_app" | base64

# Connection strings
echo -n "Host=ittrap-postgres-0.ittrap-postgres-headless;Port=5432;Database=item_tracking_app;Username=admin;Password=password@1234" | base64
echo -n "mongodb://admin:password%401234@ittrap-inventory-mongo-0.ittrap-inventory-mongo-headless:27017" | base64
```

#### Update secrets.yaml

Open [secrets.yaml](secrets.yaml) and replace all placeholders (e.g., `<JWT_PRIVATE_KEY_BASE64>`) with your base64-encoded values.

**Important**: Never commit real secrets to version control!

### 2. Review Configuration

Check [config-map.yaml](config-map.yaml) for non-sensitive configuration:
- AWS region (default: `eu-central-1`)
- Queue and topic names
- Service URLs
- Domain settings

You can modify these values if needed, but defaults work for local development.

## Quick Start

### Option 1: Using the Deploy Script (Recommended)

The easiest way to deploy everything:

```bash
# Make the script executable
chmod +x deploy.sh

# Run deployment
./deploy.sh
```

This script will:
1. Create the `ittrap` namespace
2. Create ConfigMaps for LocalStack initialization scripts
3. Apply all ConfigMaps and Secrets
4. Deploy LocalStack (AWS services emulation)
5. Deploy PostgreSQL StatefulSet
6. Deploy all microservices in the correct order

### Option 2: Manual Deployment

For learning or troubleshooting, deploy step by step:

```bash
# 1. Create namespace
kubectl create namespace ittrap

# 2. Create LocalStack init scripts ConfigMap
kubectl create configmap localstack-init-scripts \
  --from-file=./localstack \
  -n ittrap

# 3. Apply configuration
kubectl apply -n ittrap -f config-map.yaml
kubectl apply -n ittrap -f secrets.yaml

# 4. Deploy infrastructure
kubectl apply -n ittrap -f localstack.yaml
kubectl apply -n ittrap -f postgres-statefulset.yaml
kubectl apply -n ittrap -f postgres-service.yaml
kubectl apply -n ittrap -f mailhog.yaml

# Wait for databases to be ready
kubectl wait --for=condition=ready pod -l app=ittrap-postgres-service -n ittrap --timeout=300s

# 5. Deploy MongoDB
kubectl apply -n ittrap -f inventory-mongo-statefulset.yaml
kubectl apply -n ittrap -f inventory-mongo-service.yaml

# Wait for MongoDB
kubectl wait --for=condition=ready pod -l app=ittrap-inventory-mongo-service -n ittrap --timeout=300s

# 6. Deploy microservices
kubectl apply -n ittrap -f api-gateway.yaml
kubectl apply -n ittrap -f identity-service.yaml
kubectl apply -n ittrap -f email-service.yaml
kubectl apply -n ittrap -f user-service.yaml
kubectl apply -n ittrap -f inventory-service.yaml
kubectl apply -n ittrap -f location-service.yaml
kubectl apply -n ittrap -f management-service.yaml
kubectl apply -n ittrap -f query-service.yaml
```

### 3. Monitor Deployment

Watch pods starting up:

```bash
# Watch all pods in the namespace
kubectl get pods -n ittrap -w

# Check deployment status
kubectl get deployments -n ittrap

# Check StatefulSets
kubectl get statefulsets -n ittrap

# Check services
kubectl get services -n ittrap
```

Wait until all pods show `Running` status and `1/1` or `2/2` ready.

### 4. Access the Application

#### Get the API Gateway URL

**For Minikube:**
```bash
minikube service ittrap-api-gateway-nodeport -n ittrap --url
```

**For other clusters:**
```bash
# Get the node IP
kubectl get nodes -o wide

# The API Gateway is exposed on port 30080
# Access at: http://<NODE_IP>:30080
```

**For Kind/K3d:**
```bash
# Forward the port to localhost
kubectl port-forward -n ittrap service/ittrap-api-gateway-nodeport 8080:80
# Access at: http://localhost:8080
```

#### Access Other Services

```bash
# MailHog UI (Email testing)
kubectl port-forward -n ittrap service/ittrap-mailhog-nodeport 8025:8025
# Access at: http://localhost:8025

# Mongo Express (MongoDB UI)
kubectl port-forward -n ittrap service/ittrap-inventory-mongo-express-nodeport 8081:8081
# Access at: http://localhost:8081
```

## Understanding the Deployment

### Namespace Isolation

All resources are deployed in the `ittrap` namespace, providing:
- **Resource isolation** from other applications
- **Easy cleanup** - delete namespace to remove everything
- **RBAC boundaries** for access control
- **Resource quotas** can be applied per namespace

### ConfigMaps vs Secrets

**ConfigMaps** ([config-map.yaml](config-map.yaml)):
- Non-sensitive configuration
- Queue/topic names, URLs, region settings
- Can be viewed by anyone with cluster access
- Updates require pod restart to take effect

**Secrets** ([secrets.yaml](secrets.yaml)):
- Sensitive data (passwords, keys)
- Base64 encoded (not encrypted by default!)
- Mounted as environment variables or files
- Should be managed with tools like Sealed Secrets in production

### StatefulSets vs Deployments

**StatefulSets** (PostgreSQL, MongoDB):
- Stable network identities: `ittrap-postgres-0`, `ittrap-postgres-1`
- Persistent storage that survives pod restarts
- Ordered deployment and scaling
- Used for databases and stateful applications

**Deployments** (Microservices):
- Stateless applications
- Can be scaled horizontally easily
- Pods are interchangeable
- Rolling updates with zero downtime

### Service Types

**ClusterIP** (default):
- Internal-only access within the cluster
- Used for service-to-service communication
- Example: `ittrap-postgres-headless`

**NodePort**:
- Exposes service on each node's IP at a static port
- Accessible from outside the cluster
- Used for API Gateway (port 30080)

### Resource Limits

Each service has resource requests and limits:
```yaml
resources:
  requests:  # Guaranteed resources
    memory: "256Mi"
    cpu: "100m"
  limits:    # Maximum allowed
    memory: "512Mi"
    cpu: "300m"
```

Adjust these based on your cluster capacity and workload.

## Common Operations

### Viewing Logs

```bash
# Logs from a specific pod
kubectl logs -n ittrap <pod-name>

# Follow logs in real-time
kubectl logs -n ittrap -f <pod-name>

# Logs from all pods of a deployment
kubectl logs -n ittrap -l app=ittrap-api-gateway

# Logs from previous crashed container
kubectl logs -n ittrap <pod-name> --previous
```

### Executing Commands in Pods

```bash
# Get a shell in a pod
kubectl exec -n ittrap -it <pod-name> -- /bin/bash

# Run a single command
kubectl exec -n ittrap <pod-name> -- env

# Access PostgreSQL
kubectl exec -n ittrap -it ittrap-postgres-0 -- psql -U admin -d item_tracking_app

# Access MongoDB
kubectl exec -n ittrap -it ittrap-inventory-mongo-0 -- mongosh -u admin -p password@1234
```

### Scaling Services

```bash
# Scale a deployment
kubectl scale deployment ittrap-api-gateway-deployment -n ittrap --replicas=3

# Verify scaling
kubectl get pods -n ittrap -l app=ittrap-api-gateway

# Auto-scale based on CPU
kubectl autoscale deployment ittrap-api-gateway-deployment -n ittrap --min=2 --max=5 --cpu-percent=80
```

### Updating Services

```bash
# Update image version
kubectl set image deployment/ittrap-api-gateway-deployment \
  ittrap-api-gateway=docker.io/kotzakhar/ittrap-api-gateway:v2 \
  -n ittrap

# Watch rollout status
kubectl rollout status deployment/ittrap-api-gateway-deployment -n ittrap

# Rollback if needed
kubectl rollout undo deployment/ittrap-api-gateway-deployment -n ittrap
```

### Debugging Pods

```bash
# Describe pod (shows events and state)
kubectl describe pod -n ittrap <pod-name>

# Check pod status
kubectl get pod -n ittrap <pod-name> -o yaml

# View events
kubectl get events -n ittrap --sort-by='.lastTimestamp'

# Check resource usage
kubectl top pods -n ittrap
kubectl top nodes
```

## Testing the Application

### Using Postman

1. Import the Postman collection from project root
2. Set base URL to your API Gateway URL
3. Start with authentication endpoints
4. Use returned JWT token for other requests

### Testing Email Flow

1. Trigger an email action (registration, password reset)
2. Access MailHog UI at http://localhost:8025
3. View captured emails

## Performance Tuning

### Optimize Resource Allocation

Monitor actual usage:
```bash
# Install metrics server (if not available)
kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml

# Check resource usage
kubectl top pods -n ittrap
kubectl top nodes

# Adjust resources in YAML files based on actual usage
```

### Reduce Startup Time

```bash
# Pull images before deploying
docker pull kotzakhar/ittrap-api-gateway:latest
docker pull kotzakhar/ittrap-identity-service:latest
# ... pull other images

# For Minikube, load images into cluster
minikube image load kotzakhar/ittrap-api-gateway:latest

# For Kind
kind load docker-image kotzakhar/ittrap-api-gateway:latest
```

## Cleanup

### Remove All Resources

```bash
# Delete the entire namespace (removes everything)
kubectl delete namespace ittrap

# Or delete resources individually
kubectl delete -n ittrap -f .

# Delete ConfigMap
kubectl delete configmap localstack-init-scripts -n ittrap
```

### Clean Persistent Volumes

```bash
# List PVCs
kubectl get pvc -n ittrap

# Delete specific PVC
kubectl delete pvc -n ittrap <pvc-name>

# List orphaned PVs
kubectl get pv

# Delete specific PV
kubectl delete pv <pv-name>
```

### Stop Kubernetes Cluster

```bash
# Minikube
minikube stop
minikube delete  # Complete removal

# Kind
kind delete cluster

# Docker Desktop
# Stop via Docker Desktop UI
```

---

**Note**: This configuration is optimized for local development and learning. For production deployments, consider additional security measures, proper secret management (e.g., Vault, Sealed Secrets), monitoring, and infrastructure as code tools.