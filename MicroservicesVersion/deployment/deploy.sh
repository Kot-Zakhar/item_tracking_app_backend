#!/bin/bash

kubectl create configmap localstack-init-scripts \
    --from-file=../localstack

kubectl apply -f config-map.yaml

kubectl apply -f secrets.yaml

kubectl apply -f localstack.yaml

kubectl apply -f api-gateway.yaml

kubectl apply -f user-postgres-service.yaml
kubectl apply -f user-service.yaml

kubectl apply -f identity-postgres-service.yaml
kubectl apply -f identity-service.yaml
