#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TAG=${TAG:-latest}
REGISTRY=${DOCKER_REGISTRY:-docker.io}
NAMESPACE=${DOCKER_NAMESPACE:-kotzakhar}
JOBS=${JOBS:-4}

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

pids=()
names=()

build_push() {
  local project_dir=$1 image_name=$2 context=$3 full_tag=$4
  if [[ ! -d "$context" ]]; then
    echo "Skipping ${project_dir}: directory not found" >&2
    return 0
  fi
  if [[ ! -f "$context/Dockerfile" ]]; then
    echo "Skipping ${project_dir}: Dockerfile missing" >&2
    return 0
  fi

  echo "[${image_name}] Building ${full_tag}..."
  docker build --pull -t "$full_tag" "$context"

  echo "[${image_name}] Pushing ${full_tag}..."
  docker push "$full_tag"
  echo
}

for entry in "${services[@]}"; do
  IFS=":" read -r project_dir image_name <<<"$entry"
  context="${ROOT_DIR}/${project_dir}"
  full_tag="${REGISTRY}/${NAMESPACE}/${image_name}:${TAG}"

  # Throttle to at most JOBS concurrent builds
  while (( $(jobs -pr | wc -l) >= JOBS )); do
    wait -n
  done

  build_push "$project_dir" "$image_name" "$context" "$full_tag" &
  pids+=($!)
  names+=("$image_name")
done

failures=()
# Collect statuses with -e temporarily disabled so we can record failures without exiting
set +e
for i in "${!pids[@]}"; do
  if ! wait "${pids[$i]}"; then
    failures+=("${names[$i]}")
  fi
done
set -e

if (( ${#failures[@]} )); then
  echo "Failed builds/pushes: ${failures[*]}" >&2
  exit 1
fi

echo "All images pushed with tag '${TAG}' (parallel jobs: ${JOBS})."
