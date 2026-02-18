# Local Docker Deployment

This directory contains everything you need to run the entire Item Tracking App microservices architecture locally using Docker Compose.

## Overview

The application is composed of the following services:

### Core Services
- **API Gateway** (Port 50000) - Entry point for all client requests
- **Query Service** (Port 50001) - Read-optimized service for querying data
- **Identity Service** (Port 50002) - Handles authentication and authorization
- **Inventory Service** (Port 50003/60003) - Manages items and inventory
- **Management Service** (Port 50004/60004) - Administrative operations
- **Location Service** (Port 50005/60005) - Manages locations and hierarchies
- **User Service** (Port 50006/60006) - User profile management
- **Email Service** (Port 50007) - Email notifications

### Infrastructure Services
- **LocalStack** (Port 4566) - AWS services emulation (SQS, SNS)
- **PostgreSQL databases** - Separate databases for each service
  - Identity DB (Port 55432)
  - Inventory DB (Port 55433)
  - Management DB (Port 55434)
  - Location DB (Port 55435)
  - User DB (Port 55436)
- **MongoDB** (Port 27013) - Document storage for inventory
- **Mongo Express** (Port 8013) - MongoDB web UI
- **MailHog** (Port 8025) - Email testing UI

## Prerequisites

Before you begin, make sure you have installed:

- **Docker** (version 20.10 or higher)
- **Docker Compose** (version 2.0 or higher)
- At least **8GB of RAM** allocated to Docker
- At least **10GB of free disk space**

Verify your installation:
```bash
docker --version
docker compose version
```

## Environment Configuration

The application requires several environment variables. Create a `.env` file in this directory with the following configuration:

```bash
# Domain Configuration
DOMAIN=localhost

# JWT Configuration
JWT_PRIVATE_KEY=your-secret-jwt-key-here-minimum-32-characters-long

# MediatR License Key (if applicable)
MEDIATR_LICENSE_KEY=your-mediatr-license-key

# Password Security
PASSWORD_PEPPER=your-random-pepper-string

# Admin Configuration
ADMIN_EMAIL=admin@example.com
ADMIN_PHONE=+1234567890

# AWS/LocalStack Configuration
AWS_DEFAULT_REGION=us-east-1
AWS_SERVICE_URL=localstack:4566
AWS_ACCOUNT_ID=000000000000
AWS_ACCESS_KEY_ID=test
AWS_SECRET_ACCESS_KEY=test

# SQS Queue Names
MANAGEMENT_QUEUE=management-queue
AUTH_QUEUE=auth-queue
QUERY_QUEUE=query-queue

# SNS Topic Names
USER_TOPIC=user-topic
ITEM_TOPIC=item-topic
AUTH_TOPIC=auth-topic
MANAGEMENT_TOPIC=management-topic
LOCATION_TOPIC=location-topic
```

### Generate Secure Keys

For production-like testing, generate secure keys:

```bash
# Generate JWT key (at least 32 characters)
openssl rand -base64 32

# Generate password pepper
openssl rand -base64 24
```

## Quick Start

### 1. Set Up Environment Variables

Copy the example above and create your `.env` file:

```bash
cat > .env <<EOF
# Paste the configuration above and fill in your values
EOF
```

### 2. Build and Start Services

Build all Docker images and start the services:

```bash
docker compose up --build
```

For background execution:

```bash
docker compose up -d --build
```

### 3. Wait for Services to Initialize

The first startup takes 3-5 minutes as services:
- Build Docker images
- Initialize databases
- Run migrations
- Configure LocalStack (SQS/SNS)

Monitor startup progress:

```bash
docker compose logs -f
```

### 4. Verify Services are Running

Check all services are healthy:

```bash
docker compose ps
```

All services should show status as "Up" or "healthy".

### 5. Access the Application

- **API Gateway**: http://localhost:50000
- **Mongo Express** (Database UI): http://localhost:8013
- **MailHog** (Email UI): http://localhost:8025

## Service Architecture

### Communication Patterns

- **HTTP/REST**: Client-facing APIs through API Gateway
- **gRPC**: Inter-service communication (ports 60003-60006)
- **SNS/SQS**: Asynchronous event messaging via LocalStack

### Database Isolation

Each service has its own PostgreSQL database following microservices best practices. This ensures:
- Data isolation between services
- Independent scaling
- Service autonomy

## Common Tasks

### Viewing Logs

View logs for all services:
```bash
docker compose logs -f
```

View logs for a specific service:
```bash
docker compose logs -f ittrap-api-gateway
docker compose logs -f ittrap-identity-service
```

### Stopping Services

Stop all services:
```bash
docker compose down
```

Stop and remove volumes (resets databases):
```bash
docker compose down -v
```

### Rebuilding a Specific Service

```bash
docker compose up -d --build ittrap-inventory-service
```

### Restarting a Service

```bash
docker compose restart ittrap-user-service
```

### Accessing Service Shells

Access PostgreSQL:
```bash
docker compose exec ittrap-identity-postgres psql -U admin -d item_tracking_app
```

Access MongoDB:
```bash
docker compose exec ittrap-inventory-mongo-service mongosh -u admin -p 'password@1234'
```

### Inspecting Messages in LocalStack

Check SQS queues:
```bash
docker compose exec localstack awslocal sqs list-queues
docker compose exec localstack awslocal sqs receive-message --queue-url http://localhost:4566/000000000000/management-queue
```

Check SNS topics:
```bash
docker compose exec localstack awslocal sns list-topics
```

## Testing the Application

### Using Postman Collections

The repository includes Postman collections for testing:

1. Import `Item Tracking App - Microservice Version.postman_collection.json` from the root directory
2. Configure the base URL to `http://localhost:50000`
3. Start with authentication endpoints in Identity Service
4. Use the received JWT token for subsequent requests

## Cleaning Up

### Remove All Containers and Volumes

```bash
docker compose down -v --remove-orphans
```

### Free Up Disk Space

```bash
docker system prune -a --volumes
```

⚠️ **Warning**: This removes all unused Docker resources system-wide, not just this project.

## Development Workflow

### Making Code Changes

1. Modify service code in `../../src/[ServiceName]/`
2. Rebuild and restart the service:
   ```bash
   docker compose up -d --build ittrap-[service-name]
   ```

## Architecture Notes

### Service Dependencies

The startup order is managed through `depends_on` configurations:
- All services wait for their respective databases to be healthy
- Services using AWS wait for LocalStack to be ready
- Query Service waits for all gRPC services to start

### Event-Driven Communication

Services communicate asynchronously using:
- **SNS Topics**: Publish events (user created, item updated, etc.)
- **SQS Queues**: Consume events for processing
- **LocalStack**: Emulates AWS services locally

## Next Steps

- Check the main [README.md](../../README.md) for project overview
- Review [TODO.md](../../TODO.md) for development roadmap
- Explore Kubernetes deployment in [local-kubernetes/](../local-kubernetes/)
- See AWS deployment configuration in [aws/](../aws/)

## Support

For issues and questions:
1. Check service logs: `docker compose logs [service-name]`
2. Review this troubleshooting guide
3. Verify environment variables are set correctly
4. Ensure Docker has sufficient resources

---

**Note**: This setup is for local development and testing only. Do not use these configurations in production environments.