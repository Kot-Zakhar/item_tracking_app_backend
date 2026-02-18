{{/*
Build an SQS queue URL.
For LocalStack: http://<serviceUrl>/queue/<region>/000000000000/<queue-name>
For real AWS:   https://sqs.<region>.amazonaws.com/<account-id>/<queue-name>
*/}}
{{- define "ittrap.sqsQueueUrl" -}}
{{- if .global.aws.serviceUrl -}}
{{ .global.aws.serviceUrl }}/queue/$(AWS_DEFAULT_REGION)/$(AWS_ACCOUNT_ID)/{{ .queueName }}
{{- else -}}
https://sqs.$(AWS_DEFAULT_REGION).amazonaws.com/$(AWS_ACCOUNT_ID)/{{ .queueName }}
{{- end -}}
{{- end -}}

{{/*
Build an SNS topic ARN.
*/}}
{{- define "ittrap.snsTopicArn" -}}
arn:aws:sns:$(AWS_DEFAULT_REGION):$(AWS_ACCOUNT_ID):{{ .topicName }}
{{- end -}}

{{/*
Build an IAM role ARN.
*/}}
{{- define "ittrap.iamRoleArn" -}}
arn:aws:iam::$(AWS_ACCOUNT_ID):role/{{ .roleName }}
{{- end -}}