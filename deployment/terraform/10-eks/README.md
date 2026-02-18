# 10-eks

Creates:
- VPC (public subnets; optional private subnets + NAT)
- EKS cluster
- Managed node group

This uses community modules to keep the amount of code small so you can focus on concepts.

## Learning mode (cost-optimized defaults)

This stage defaults to `learning_mode=true` to reduce costs:
- uses only **public subnets** (no private subnets)
- **disables NAT Gateway** (a common “silent” cost driver)
- uses a smaller node group (`t3.micro`, 1 node by default)

Tradeoffs:
- not production-grade network isolation
- EKS control plane still costs money per hour

## Backend config

Copy `backend.hcl.example` → `backend.hcl` and fill it in.

## Usage

```bash
terraform init -backend-config=backend.hcl
terraform apply
```

After apply, use the `configure_kubectl` output to connect kubectl to your cluster.
