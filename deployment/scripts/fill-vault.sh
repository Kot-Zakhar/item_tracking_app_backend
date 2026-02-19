# Load secrets from env file (copy fill-vault.env.example -> fill-vault.env and fill in your values)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ENV_FILE="$SCRIPT_DIR/fill-vault.env"

if [[ ! -f "$ENV_FILE" ]]; then
  echo "Error: $ENV_FILE not found. Copy fill-vault.env.example to fill-vault.env and fill in your values."
  exit 1
fi

# shellcheck source=fill-vault.env
source "$ENV_FILE"

# Exec into vault pod
kubectl exec -it -n vault vault-0 -- sh

# Inside vault pod - enable KV v2 engine (if not already enabled)
vault secrets enable -path=kv kv-v2

# Populate all secrets
vault kv put kv/ittrap/global \
  jwt-private-key="$JWT_PRIVATE_KEY" \
  mediatr-license-key="$MEDIATR_LICENSE_KEY" \
  admin-email="$ADMIN_EMAIL" \
  admin-phone="$ADMIN_PHONE"

vault kv put kv/ittrap/postgres \
  postgres-user="$POSTGRES_USER" \
  postgres-password="$POSTGRES_PASSWORD" \
  postgres-database="$POSTGRES_DATABASE"

vault kv put kv/ittrap/user-service-config \
  postgreSqlConnectionString="$USER_SERVICE_POSTGRES_CONNECTION_STRING"

vault kv put kv/ittrap/identity-service-config \
  postgreSqlConnectionString="$IDENTITY_SERVICE_POSTGRES_CONNECTION_STRING" \
  passwordPepper="$IDENTITY_SERVICE_PASSWORD_PEPPER"

vault kv put kv/ittrap/inventory-service-config \
  postgreSqlConnectionString="$INVENTORY_SERVICE_POSTGRES_CONNECTION_STRING" \
  mongoDbConnectionString="$INVENTORY_SERVICE_MONGO_CONNECTION_STRING"

vault kv put kv/ittrap/mongo \
  mongo-initdb-root-username="$MONGO_INITDB_ROOT_USERNAME" \
  mongo-initdb-root-password="$MONGO_INITDB_ROOT_PASSWORD"

vault kv put kv/ittrap/location-service-config \
  postgreSqlConnectionString="$LOCATION_SERVICE_POSTGRES_CONNECTION_STRING"

vault kv put kv/ittrap/management-service-config \
  postgreSqlConnectionString="$MANAGEMENT_SERVICE_POSTGRES_CONNECTION_STRING"

vault kv put kv/ittrap/smtp \
  host="$SMTP_HOST" \
  port="$SMTP_PORT" \
  username="$SMTP_USERNAME" \
  password="$SMTP_PASSWORD" \
  fromEmail="$SMTP_FROM_EMAIL" \
  fromName="$SMTP_FROM_NAME" \
  useSsl="$SMTP_USE_SSL"

vault kv put kv/ittrap/aws \
  access-key-id="$AWS_ACCESS_KEY_ID" \
  secret-access-key="$AWS_SECRET_ACCESS_KEY"