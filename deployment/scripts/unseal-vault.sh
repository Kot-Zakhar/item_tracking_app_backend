#!/bin/bash

set -e

VAULT_NAMESPACE="vault"
VAULT_POD="vault-0"

echo "Checking Vault status..."
STATUS=$(kubectl exec -n $VAULT_NAMESPACE $VAULT_POD -- vault status -format=json 2>/dev/null || true)

SEALED=$(echo $STATUS | grep -o '"sealed":[^,}]*' | cut -d: -f2 | tr -d ' ')
INITIALIZED=$(echo $STATUS | grep -o '"initialized":[^,}]*' | cut -d: -f2 | tr -d ' ')

if [ "$INITIALIZED" != "true" ]; then
  echo "Vault is not initialized. Initializing..."
  INIT_OUTPUT=$(kubectl exec -n $VAULT_NAMESPACE $VAULT_POD -- vault operator init -format=json)

  echo "$INIT_OUTPUT" > vault-init-keys.json
  echo "⚠️  Init keys saved to vault-init-keys.json — keep this file safe and do not commit it!"

  UNSEAL_KEY_1=$(echo "$INIT_OUTPUT" | jq -r '.unseal_keys_b64[0]')
  UNSEAL_KEY_2=$(echo "$INIT_OUTPUT" | jq -r '.unseal_keys_b64[1]')
  UNSEAL_KEY_3=$(echo "$INIT_OUTPUT" | jq -r '.unseal_keys_b64[2]')
  ROOT_TOKEN=$(echo "$INIT_OUTPUT" | jq -r '.root_token')

  echo "Root token: $ROOT_TOKEN"
else
  if [ "$SEALED" != "true" ]; then
    echo "Vault is already unsealed."
    exit 0
  fi

  if [ ! -f vault-init-keys.json ]; then
    echo "vault-init-keys.json not found. Please provide unseal keys manually."
    read -sp "Unseal Key 1: " UNSEAL_KEY_1; echo
    read -sp "Unseal Key 2: " UNSEAL_KEY_2; echo
    read -sp "Unseal Key 3: " UNSEAL_KEY_3; echo
  else
    echo "Reading keys from vault-init-keys.json..."
    UNSEAL_KEY_1=$(jq -r '.unseal_keys_b64[0]' vault-init-keys.json)
    UNSEAL_KEY_2=$(jq -r '.unseal_keys_b64[1]' vault-init-keys.json)
    UNSEAL_KEY_3=$(jq -r '.unseal_keys_b64[2]' vault-init-keys.json)
  fi
fi

echo "Unsealing Vault..."
kubectl exec -n $VAULT_NAMESPACE $VAULT_POD -- vault operator unseal "$UNSEAL_KEY_1"
kubectl exec -n $VAULT_NAMESPACE $VAULT_POD -- vault operator unseal "$UNSEAL_KEY_2"
kubectl exec -n $VAULT_NAMESPACE $VAULT_POD -- vault operator unseal "$UNSEAL_KEY_3"

echo "Vault unsealed successfully!"
kubectl exec -n $VAULT_NAMESPACE $VAULT_POD -- vault status