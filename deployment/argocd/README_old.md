# Argo CD setup for this repo (Helm)

This directory contains Argo CD `Application` manifests for deploying the Helm chart in `deployment/local-kubernetes-argocd/helm/ittrap`.

## 1) Install Argo CD (one-time)

Example using Helm:

```bash
kubectl create namespace argocd
helm repo add argo https://argoproj.github.io/argo-helm
helm repo update
helm install argocd argo/argo-cd -n argocd
```

## 2) Create a root "app-of-apps" (recommended)

```bash
kubectl apply -n argocd -f deployment/local-kubernetes-argocd/argocd/root-app.yaml
```

Argo CD will then create/sync the child apps from `deployment/local-kubernetes-argocd/argocd/apps/`.
