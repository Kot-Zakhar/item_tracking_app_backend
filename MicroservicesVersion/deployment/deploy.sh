#!/bin/bash

kubectl create namespace ittrap

kubectl create configmap localstack-init-scripts --from-file=../localstack -n ittrap

kubectl apply -n ittrap -f config-map.yaml

kubectl apply -n ittrap -f secrets.yaml

kubectl apply -n ittrap -f localstack.yaml

kubectl apply -n ittrap -f api-gateway.yaml

kubectl apply -n ittrap -f user-postgres-statefulset.yaml
kubectl apply -n ittrap -f user-postgres-service.yaml
kubectl apply -n ittrap -f user-service.yaml

kubectl apply -n ittrap -f identity-postgres-statefulset.yaml
kubectl apply -n ittrap -f identity-postgres-service.yaml
kubectl apply -n ittrap -f identity-service.yaml

kubectl apply -n ittrap -f email-service.yaml
kubectl apply -n ittrap -f mailhog.yaml

kubectl apply -n ittrap -f query-service.yaml

kubectl apply -n ittrap -f inventory-postgres-statefulset.yaml
kubectl apply -n ittrap -f inventory-postgres-service.yaml
kubectl apply -n ittrap -f inventory-mongo-statefulset.yaml
kubectl apply -n ittrap -f inventory-mongo-service.yaml
kubectl apply -n ittrap -f inventory-service.yaml

kubectl apply -n ittrap -f location-postgres-statefulset.yaml
kubectl apply -n ittrap -f location-postgres-service.yaml
kubectl apply -n ittrap -f location-service.yaml

kubectl apply -n ittrap -f management-postgres-statefulset.yaml
kubectl apply -n ittrap -f management-postgres-service.yaml
kubectl apply -n ittrap -f management-service.yaml


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
#   location-service-config:
#     outbound-sns-topic-arc=arn:aws:sns:us-east-1:000000000000:location-events
#   management-service-config:
#     outbound-sns-topic-arc=arn:aws:sns:us-east-1:000000000000:management-events
#     sqs-url=http://ittrap-localstack-service:4566/000000000000/management-queue


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
#   location-service-config:
#     postgreSqlConnectionString
#   location-postgres-config:
#     postgres-user
#     postgres-password
#     postgres-database
#   management-service-config:
#     postgreSqlConnectionString
#   management-postgres-config:
#     postgres-user
#     postgres-password
#     postgres-database
#   aws-credentials:
#     access-key-id
#     secret-access-key
