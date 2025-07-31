#!/usr/bin/env bash
set -e

# create SNS topics
awslocal sns create-topic --name $USER_TOPIC
awslocal sns create-topic --name $AUTH_TOPIC
awslocal sns create-topic --name $ITEM_TOPIC
awslocal sns create-topic --name $LOCATION_TOPIC

# create the SQS queue
awslocal sqs create-queue --queue-name $MANAGEMENT_QUEUE
awslocal sqs create-queue --queue-name $AUTH_QUEUE

# fetch ARNs & URLs
USER_ARN=$(awslocal sns list-topics --query "Topics[?ends_with(TopicArn, ':$USER_TOPIC')].TopicArn" --output text)
AUTH_ARN=$(awslocal sns list-topics --query "Topics[?ends_with(TopicArn, ':$AUTH_TOPIC')].TopicArn" --output text)
ITEM_ARN=$(awslocal sns list-topics --query "Topics[?ends_with(TopicArn, ':$ITEM_TOPIC')].TopicArn" --output text)
LOCATION_ARN=$(awslocal sns list-topics --query "Topics[?ends_with(TopicArn, ':$LOCATION_TOPIC')].TopicArn" --output text)

MANAGEMENT_QUEUE_URL=$(awslocal sqs get-queue-url --queue-name $MANAGEMENT_QUEUE --output text)
MANAGEMENT_QUEUE_ARN=$(awslocal sqs get-queue-attributes --queue-url "$MANAGEMENT_QUEUE_URL" --attribute-names QueueArn \
            --query "Attributes.QueueArn" --output text)

# subscribe the queue to each topic
for TOPIC_ARN in "$USER_ARN" "$ITEM_ARN" "$LOCATION_ARN"; do
  awslocal sns subscribe --topic-arn "$TOPIC_ARN" --protocol sqs --notification-endpoint "$MANAGEMENT_QUEUE_ARN"
done

# set queue policy so SNS can publish to it
echo $(cat <<EOF
{
  "Policy": "{
    \"Version\": \"2012-10-17\",
    \"Statement\": [
      {
        \"Effect\": \"Allow\",
        \"Principal\": \"*\",
        \"Action\": \"sqs:SendMessage\",
        \"Resource\": \"$MANAGEMENT_QUEUE_ARN\",
        \"Condition\": {
          \"ArnEquals\": {
            \"aws:SourceArn\": [
              \"$USER_ARN\",
              \"$ITEM_ARN\",
              \"$LOCATION_ARN\"
            ]
          }
        }
      }
    ]
  }"
}
EOF
) > management_policy.json

awslocal sqs set-queue-attributes --queue-url "$MANAGEMENT_QUEUE_URL" --attributes file://management_policy.json


AUTH_QUEUE_URL=$(awslocal sqs get-queue-url --queue-name $AUTH_QUEUE --output text)
AUTH_QUEUE_ARN=$(awslocal sqs get-queue-attributes --queue-url "$AUTH_QUEUE_URL" --attribute-names QueueArn \
            --query "Attributes.QueueArn" --output text)

awslocal sns subscribe --topic-arn "$USER_ARN" --protocol sqs --notification-endpoint "$AUTH_QUEUE_ARN"

# set queue policy so SNS can publish to it
echo $(cat <<EOF
{
  "Policy": "{
    \"Version\": \"2012-10-17\",
    \"Statement\": [
      {
        \"Effect\": \"Allow\",
        \"Principal\": \"*\",
        \"Action\": \"sqs:SendMessage\",
        \"Resource\": \"$AUTH_QUEUE_ARN\",
        \"Condition\": {
          \"ArnEquals\": {
            \"aws:SourceArn\": [
              \"$USER_ARN\"
            ]
          }
        }
      }
    ]
  }"
}
EOF
) > auth_policy.json

awslocal sqs set-queue-attributes --queue-url "$AUTH_QUEUE_URL" --attributes file://auth_policy.json