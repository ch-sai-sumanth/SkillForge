# SkillForge - Mentorship Platform API

A comprehensive mentorship platform built with .NET 8, following Clean Architecture and CQRS patterns. SkillForge connects mentees with mentors, enabling skill development through structured learning paths, sessions, and task management.

## 🚀 Features

### Core Functionality
- **User Management**: Registration, authentication, profile management with JWT tokens
- **Mentorship Matching**: Skill-based mentor-mentee matching system
- **Session Management**: Book, schedule, and manage mentoring sessions
- **Learning Goals & Tasks**: Create and track learning objectives with assignable tasks
- **Payment System**: Subscription plans and payment processing for premium features
- **Mentor Rates**: Flexible pricing system for mentor services

### Advanced Features
- **Session History**: Complete tracking of mentoring sessions
- **File Uploads**: Profile picture management
- **Real-time Logging**: Structured logging with Serilog
- **API Validation**: Comprehensive request/response validation
- **Integration Testing**: Full test coverage with schema validation

## 🏗️ Architecture

SkillForge follows Clean Architecture principles with clear separation of concerns:

```
src/
├── Core/
│   ├── Domain/           # Domain entities and business logic
│   │   ├── Entities/     # Core business entities
│   │   └── Enums/        # Domain enumerations
│   └── Application/      # Application services and contracts
│       ├── Commands/     # CQRS Command handlers
│       ├── Queries/      # CQRS Query handlers
│       ├── DTOs/         # Data Transfer Objects
│       ├── Services/     # Application services
│       └── Interfaces/   # Service contracts
├── Infrastructure/
│   └── Persistence/      # Data access layer (MongoDB)
└── Presentation/
    └── WebApi/          # REST API controllers and configuration
```

## 🛠️ Technologies

- **.NET 8** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **MongoDB** - NoSQL database
- **JWT Authentication** - Secure token-based authentication
- **MediatR** - CQRS implementation
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Serilog** - Structured logging
- **BCrypt** - Password hashing
- **Swagger/OpenAPI** - API documentation

## 📋 Prerequisites

### For Local Development
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MongoDB](https://www.mongodb.com/try/download/community) (local or cloud instance)

### For Docker Deployment (Recommended)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/) (included with Docker Desktop)

## 🚀 Getting Started

### Option 1: Docker (Recommended)

```bash
# 1. Clone the repository
git clone https://github.com/yourusername/SkillForge.git
cd SkillForge

# 2. Start the application
docker compose up --build -d

# 3. Access the application
# API: http://localhost:8081
# Swagger: http://localhost:8081/swagger
# MongoDB: localhost:27018
```

### Option 2: Local Development

#### 1. Clone the Repository
```bash
git clone [https://github.com/ch-sai-sumanth/SkillForge.git](https://github.com/ch-sai-sumanth/SkillForge.git)
cd SkillForge
```

#### 2. Configuration
Update the connection string in `src/Presentation/WebApi/appsettings.Development.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "SkillForgeDB"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-here-must-be-at-least-32-characters",
    "Issuer": "SkillForge",
    "Audience": "SkillForge-Users",
    "ExpirationMinutes": 60
  }
}
```

#### 3. Restore Dependencies
```bash
dotnet restore
```

#### 4. Run the Application
```bash
dotnet run --project src/Presentation/WebApi
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `http://localhost:5000/swagger`

> **Note**: HTTPS redirection is disabled in Docker deployments. For local development, use HTTP endpoints.

### Port Configuration Summary

| Deployment Type | API Port | Swagger | MongoDB | MongoDB Express |
|----------------|----------|---------|---------|-----------------|
| Docker Compose | 8081 | http://localhost:8081/swagger | 27018 | 8082 (dev profile) |
| Local Development | 5000 | http://localhost:5000/swagger | 27017 | N/A |

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test tests/Application.UnitTests/
```

### Run Integration Tests
```bash
# Make sure Docker is running for MongoDB containers
chmod +x run-validation-tests.sh
./run-validation-tests.sh
```

The integration tests include comprehensive API validation with JSON schema verification to ensure API contracts remain stable during refactoring.

## 📚 API Documentation

Once the application is running, visit the Swagger UI at `http://localhost:5000/swagger` (local development) or `http://localhost:8081/swagger` (Docker) for interactive API documentation.

### Key Endpoints

#### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh-token` - Refresh JWT token
- `POST /api/auth/logout` - User logout

#### User Management
- `GET /api/profile` - Get user profile
- `PUT /api/profile` - Update user profile
- `POST /api/profile/upload-picture` - Upload profile picture

#### Sessions
- `POST /api/session/book` - Book a mentoring session
- `GET /api/session/user-sessions` - Get user's sessions
- `PUT /api/session/accept/{sessionId}` - Accept session (mentor)
- `PUT /api/session/complete/{sessionId}` - Mark session complete

#### Goals & Tasks
- `POST /api/goal` - Create learning goal
- `GET /api/goal/mentee/{menteeId}` - Get mentee's goals
- `POST /api/task/assign` - Assign task to mentee
- `GET /api/task/mentee/{menteeId}` - Get mentee's tasks

## 🔧 Development

### Project Structure
- **Clean Architecture** - Separation of concerns with clear dependencies
- **CQRS Pattern** - Command Query Responsibility Segregation
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Built-in .NET DI container
- **Validation** - FluentValidation for request validation

### Adding New Features
1. Define domain entities in `Core/Domain/Entities/`
2. Create DTOs in `Core/Application/DTOs/`
3. Implement commands/queries in `Core/Application/Commands|Queries/`
4. Add repository interfaces and implementations
5. Create API controllers in `Presentation/WebApi/Controllers/`
6. Add integration tests

### Code Quality
The project includes:
- **Response validation middleware** (development only)
- **Exception handling middleware**
- **Structured logging**
- **API schema validation**
- **Comprehensive test coverage**

## 🐳 Docker Deployment

### Quick Start with Docker Compose
The easiest way to run SkillForge is using Docker Compose:

```bash
# Clone the repository
git clone https://github.com/yourusername/SkillForge.git
cd SkillForge

# Start all services (API + MongoDB)
docker compose up --build -d

# View logs
docker compose logs -f skillforge-api

# Stop all services
docker compose down
```

**That's it!** The API will be available at `http://localhost:8081` and Swagger UI at `http://localhost:8081/swagger`.

### Individual Docker Commands

If you prefer to build and run manually:

```bash
# Build the Docker image
docker build -t skillforge-api .

# Run with MongoDB (using Docker network)
docker network create skillforge-network

# Start MongoDB
docker run -d \
  --name mongodb \
  --network skillforge-network \
  -p 27018:27017 \
  -e MONGO_INITDB_ROOT_USERNAME=admin \
  -e MONGO_INITDB_ROOT_PASSWORD=password123 \
  mongo:7.0

# Start SkillForge API
docker run -d \
  --name skillforge-api \
  --network skillforge-network \
  -p 8081:8080 \
  -e MongoDbSettings__ConnectionString="mongodb://admin:password123@mongodb:27017/SkillForgeDB?authSource=admin" \
  -e JwtSettings__SecretKey="your-super-secret-jwt-key-must-be-at-least-32-characters-long" \
  skillforge-api
```

### Docker Hub
Pull the pre-built image from Docker Hub:

```bash
# Pull the latest image
docker pull yourusername/skillforge-api:latest

# Run with custom environment variables
docker run -d \
  -p 8081:8080 \
  -e MongoDbSettings__ConnectionString="your-mongodb-connection-string" \
  -e JwtSettings__SecretKey="your-secret-key" \
  yourusername/skillforge-api:latest
```

### Development with Docker Compose
For development with MongoDB Express (database UI):

```bash
# Start with development profile (includes MongoDB Express)
docker compose --profile dev up --build -d

# Access MongoDB Express at http://localhost:8082
```

### Environment Variables
Configure these environment variables for production:

| Variable | Description | Example |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Production` |
| `MongoDbSettings__ConnectionString` | MongoDB connection string | `mongodb://user:pass@host:27017/db` |
| `MongoDbSettings__DatabaseName` | Database name | `SkillForgeDB` |
| `JwtSettings__SecretKey` | JWT signing key (32+ chars) | `your-super-secret-key...` |
| `JwtSettings__Issuer` | JWT issuer | `SkillForge` |
| `JwtSettings__Audience` | JWT audience | `SkillForge-Users` |
| `JwtSettings__ExpirationMinutes` | Token expiration | `60` |

## 📊 Monitoring

The application includes structured logging with Serilog:
- Console logging for development
- Seq integration for structured log analysis
- Request/response logging middleware

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Run tests and ensure they pass
5. Push to the branch (`git push origin feature/amazing-feature`)
6. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙋‍♂️ Support

For questions or support:
- Create an issue in the GitHub repository
- Check the API documentation at `/swagger`
- Review the integration tests for usage examples

## 🔮 Future Enhancements

- Real-time messaging between mentors and mentees
- Video call integration
- Mobile application support
- Advanced analytics and reporting
- Multi-language support
- AI-powered mentor matching

---

**Built with ❤️ using .NET 8 and Clean Architecture principles**
