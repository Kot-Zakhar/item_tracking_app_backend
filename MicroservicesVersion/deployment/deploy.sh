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

kubectl apply -f email-service.yaml
kubectl apply -f mailhog.yaml

# ConfigMap:
#   global-config:
#     domain
#     aws-default-region=us-east-1
#     aws-account-id=000000000000
#     aws-service-url=http://localstack:4566
#   user-service-config:
#     outbound-sns-topic-arn=arn:aws:sns:us-east-1:000000000000:user-events
#   identity-service-config:
#     outbound-sns-topic-arn=arn:aws:sns:us-east-1:000000000000:auth-events

#   localstack-config:
#     default-region
#     management-queue
#     auth-queue
#     user-topic
#     item-topic
#     auth-topic
#     location-topic
#     localstack-host

# SecretKey:
#   global-config:
#     jwt-private-key
#     mediatr-license-key
#     admin-email
#     admin-phone
#   user-service-config:
#     postgreSqlConnectionString="Host=user-postgres-service;Port=5432;Database=item_tracking_app;Username=admin;Password=password@1234"
#   user-postgres-config:
#     postgres-user=admin
#     postgres-password=password@1234
#     postgres-database=item_tracking_app
#   identity-service-config:
#     postgreSqlConnectionString="Host=identity-postgres-service;Port=5432;Database=item_tracking_app;Username=admin;Password=password@1234"
#   aws-credentials:
#     access-key-id
#     secret-access-key