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

kubectl apply -f query-service.yaml

kubectl apply -f inventory-postgres-service.yaml
kubectl apply -f inventory-mongo-service.yaml
kubectl apply -f inventory-service.yaml

# kubectl apply -f location-postgres-service.yaml
# kubectl apply -f location-service.yaml

# kubectl apply -f management-postgres-service.yaml
# kubectl apply -f management-service.yaml


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
#     sqs-url=http://ittrap-localstack-service:4566/000000000000/auth-queue
#   query-service-config:
#     sqs-url=http://ittrap-localstack-service:4566/000000000000/query-queue
#   inventory-service-config:
#     outbound-sns-topic-arn=arn:aws:sns:us-east-1:000000000000:item-events

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
#     postgreSqlConnectionString
#   user-postgres-config:
#     postgres-user
#     postgres-password
#     postgres-database
#   identity-service-config:
#     postgreSqlConnectionString
#   identity-postgres-config:
#     postgres-user
#     postgres-password
#     postgres-database
#   inventory-service-config:
#     postgreSqlConnectionString
#     mongoDbConnectionString
#   inventory-postgres-config:
#     postgres-user
#     postgres-password
#     postgres-database
#   inventory-mongo-config:
#     mongo-initdb-root-username
#     mongo-initdb-root-password
#   aws-credentials:
#     access-key-id
#     secret-access-key