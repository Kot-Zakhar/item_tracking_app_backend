#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TAG=${TAG:-latest}
REGISTRY=${DOCKER_REGISTRY:-docker.io}
NAMESPACE=${DOCKER_NAMESPACE:-kotzakhar}

if ! command -v docker >/dev/null 2>&1; then
  echo "docker command not found; please install Docker before running." >&2
  exit 1
fi

if [[ -n "${DOCKER_USERNAME:-}" && -n "${DOCKER_PASSWORD:-}" ]]; then
  echo "Logging into ${REGISTRY} as ${DOCKER_USERNAME}..."
  echo "$DOCKER_PASSWORD" | docker login "$REGISTRY" -u "$DOCKER_USERNAME" --password-stdin
fi

services=(
  "ItTrAp.ApiGateway:ittrap-api-gateway"
  "ItTrAp.UserService:ittrap-user-service"
  "ItTrAp.IdentityService:ittrap-identity-service"
  "ItTrAp.EmailService:ittrap-email-service"
  "ItTrAp.QueryService:ittrap-query-service"
  "ItTrAp.InventoryService:ittrap-inventory-service"
  "ItTrAp.LocationService:ittrap-location-service"
  "ItTrAp.ManagementService:ittrap-management-service"
)

for entry in "${services[@]}"; do
  IFS=":" read -r project_dir image_name <<<"$entry"
  context="${ROOT_DIR}/${project_dir}"
  full_tag="${REGISTRY}/${NAMESPACE}/${image_name}:${TAG}"

  if [[ ! -d "$context" ]]; then
    echo "Skipping ${project_dir}: directory not found" >&2
    continue
  fi
  if [[ ! -f "$context/Dockerfile" ]]; then
    echo "Skipping ${project_dir}: Dockerfile missing" >&2
    continue
  fi

  echo "Building ${full_tag}..."
  docker build --pull -t "$full_tag" "$context"

  echo "Pushing ${full_tag}..."
  docker push "$full_tag"
  echo

done

echo "All images pushed with tag '${TAG}'."
