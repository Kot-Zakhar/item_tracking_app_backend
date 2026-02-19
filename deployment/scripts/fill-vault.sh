# Load secrets from env file (copy fill-vault.env.example -> fill-vault.env and fill in your values)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ENV_FILE="$SCRIPT_DIR/fill-vault.env"

if [[ ! -f "$ENV_FILE" ]]; then
  echo "Error: $ENV_FILE not found. Copy fill-vault.env.example to fill-vault.env and fill in your values."
  exit 1
fi

# shellcheck source=fill-vault.env
source "$ENV_FILE"

KUBECTL_EXEC="kubectl exec -i -n vault vault-0 -- env VAULT_TOKEN=$ROOT_TOKEN"

VAULT="$KUBECTL_EXEC vault"

# Read root token from vault-init-keys.json
VAULT_KEYS_FILE="$SCRIPT_DIR/vault-init-keys.json"
if [ ! -f "$VAULT_KEYS_FILE" ]; then
  echo "Error: $VAULT_KEYS_FILE not found. Run unseal-vault.sh first."
  exit 1
fi
ROOT_TOKEN=$(jq -r '.root_token' "$VAULT_KEYS_FILE")

# Authenticate
$VAULT login "$ROOT_TOKEN"

# Enable KV v2 engine (if not already enabled)
$VAULT secrets enable -path=kv kv-v2

# ── Populate secrets ──────────────────────────────────────────────────────────

$VAULT kv put kv/ittrap/global \
  jwt-private-key="$JWT_PRIVATE_KEY" \
  mediatr-license-key="$MEDIATR_LICENSE_KEY" \
  admin-email="$ADMIN_EMAIL" \
  admin-phone="$ADMIN_PHONE"

$VAULT kv put kv/postgres \
  postgres-user="$POSTGRES_USER" \
  postgres-password="$POSTGRES_PASSWORD" \
  postgres-database="$POSTGRES_DATABASE"

$VAULT kv put kv/ittrap/user-service-config \
  postgreSqlConnectionString="$USER_SERVICE_POSTGRES_CONNECTION_STRING"

$VAULT kv put kv/ittrap/identity-service-config \
  postgreSqlConnectionString="$IDENTITY_SERVICE_POSTGRES_CONNECTION_STRING" \
  passwordPepper="$IDENTITY_SERVICE_PASSWORD_PEPPER"

$VAULT kv put kv/ittrap/inventory-service-config \
  postgreSqlConnectionString="$INVENTORY_SERVICE_POSTGRES_CONNECTION_STRING" \
  mongoDbConnectionString="$INVENTORY_SERVICE_MONGO_CONNECTION_STRING"

$VAULT kv put kv/mongo \
  mongo-initdb-root-username="$MONGO_INITDB_ROOT_USERNAME" \
  mongo-initdb-root-password="$MONGO_INITDB_ROOT_PASSWORD"

$VAULT kv put kv/ittrap/location-service-config \
  postgreSqlConnectionString="$LOCATION_SERVICE_POSTGRES_CONNECTION_STRING"

$VAULT kv put kv/ittrap/management-service-config \
  postgreSqlConnectionString="$MANAGEMENT_SERVICE_POSTGRES_CONNECTION_STRING"

$VAULT kv put kv/ittrap/smtp \
  host="$SMTP_HOST" \
  port="$SMTP_PORT" \
  username="$SMTP_USERNAME" \
  password="$SMTP_PASSWORD" \
  fromEmail="$SMTP_FROM_EMAIL" \
  fromName="$SMTP_FROM_NAME" \
  useSsl="$SMTP_USE_SSL"

$VAULT kv put kv/ittrap/aws \
  access-key-id="$AWS_ACCESS_KEY_ID" \
  secret-access-key="$AWS_SECRET_ACCESS_KEY"

# ── Vault policies ────────────────────────────────────────────────────────────

# ittrap: all app service config paths
$VAULT policy write ittrap-policy - <<EOF
path "kv/data/ittrap/*" { capabilities = ["read"] }
EOF

# postgres namespace: read kv/postgres only
$VAULT policy write postgres-policy - <<EOF
path "kv/data/postgres" { capabilities = ["read"] }
EOF

# mongo namespace: read kv/mongo only
$VAULT policy write mongo-policy - <<EOF
path "kv/data/mongo" { capabilities = ["read"] }
EOF

# ── Kubernetes auth roles ─────────────────────────────────────────────────────

$VAULT auth enable kubernetes

$KUBECTL_EXEC sh -c 'vault write auth/kubernetes/config kubernetes_host="https://$KUBERNETES_PORT_443_TCP_ADDR:443"'

$VAULT write auth/kubernetes/role/ittrap-eso \
  bound_service_account_names=eso-vault-auth \
  bound_service_account_namespaces=ittrap \
  policies=ittrap-policy \
  ttl=1h

$VAULT write auth/kubernetes/role/postgres-eso \
  bound_service_account_names=eso-vault-auth \
  bound_service_account_namespaces=postgres \
  policies=postgres-policy \
  ttl=1h

$VAULT write auth/kubernetes/role/mongo-eso \
  bound_service_account_names=eso-vault-auth \
  bound_service_account_namespaces=mongo \
  policies=mongo-policy \
  ttl=1h