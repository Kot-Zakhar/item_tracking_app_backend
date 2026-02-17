# Monthly cost estimate (AWS/EKS demo)

This document estimates **monthly AWS costs** for keeping this demo **always-on** in **`eu-central-1`** using:
- `deployment/terraform/10-eks` in **learning mode** (public subnets, **no NAT Gateway**, small node group)
- `deployment/terraform/20-platform` with **single-AZ RDS Postgres enabled**
- An internet-facing entrypoint via **AWS Load Balancer Controller + ALB Ingress**

These numbers are **estimates**. AWS pricing changes and depends on your exact configuration/usage. Use the **AWS Pricing Calculator** to confirm before you commit to an always-on demo.

## Assumptions

- Region: `eu-central-1` (Frankfurt)
- Uptime: **730 hours/month**
- Learning mode: **enabled** (`learning_mode=true`) → **no NAT Gateway**
- Availability Zones: **2** (learning mode default)
- Nodes: `t3.micro`, 1 or 2 nodes (two scenarios below)
- EBS root volume per node: **20 GiB gp3**
- RDS: `db.t3.micro`, single AZ, **20 GiB storage**
- ALB: **1** internet-facing ALB, light traffic (~**1 LCU average**)

## Price inputs used (replace with current AWS rates if you want precision)

These are the “unit prices” used for the calculations below:

- EKS control plane: **$0.10 / hour**
- EC2 `t3.micro` (eu-central-1): **$0.012 / hour**
- Public IPv4: **$0.005 / hour per IPv4**
- EBS gp3: **$0.10 / GB-month**
- ALB hours: **$0.027 / hour**
- ALB LCU: **$0.008 / LCU-hour** (assuming 1 LCU average)
- RDS Postgres `db.t3.micro`: **$0.019 / hour**
- RDS storage: **$0.115 / GB-month**

If you want, I can change this doc to reference variables/outputs or add a tiny script to compute totals from a `costs.auto.tfvars`-like file.

## Fixed monthly costs (independent of node count)

- EKS control plane: `0.10 * 730 = $73.00`
- ALB hours: `0.027 * 730 = $19.71`
- ALB LCU (1 LCU avg): `0.008 * 730 = $5.84`
- RDS compute: `0.019 * 730 = $13.87`
- RDS storage (20 GB): `0.115 * 20 = $2.30`

Subtotal fixed: **$114.72 / month**

## Variable monthly costs (per node)

Per `t3.micro` worker node:
- EC2: `0.012 * 730 = $8.76`
- EBS gp3 (20 GB): `0.10 * 20 = $2.00`
- Public IPv4 (1 per node): `0.005 * 730 = $3.65`

Per-node subtotal: **$14.41 / month**

## Scenario A: 1 worker node

Assume ALB uses **2 public IPv4 addresses** (one per AZ).

- Fixed subtotal: **$114.72**
- Nodes: `1 * $14.41 = $14.41`
- Public IPv4 for ALB: `2 * (0.005 * 730) = $7.30`

Estimated total: **$136.43 / month**

## Scenario B: 2 worker nodes

- Fixed subtotal: **$114.72**
- Nodes: `2 * $14.41 = $28.82`
- Public IPv4 for ALB: **$7.30**

Estimated total: **$150.84 / month**

## Big cost pitfalls (things you *don’t* have in learning mode)

- NAT Gateway: adds a meaningful fixed hourly cost **plus** per-GB processed. If you disable `learning_mode`, expect your baseline to increase.

## Other costs not included (usage-dependent)

- Container registry storage (ECR), image pulls
- CloudWatch logs/metrics (can grow quickly)
- Data transfer to the internet (demo traffic)
- Secrets Manager ($/secret/month) if you store many secrets there

