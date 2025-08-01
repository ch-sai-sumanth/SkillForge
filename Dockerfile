# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY SkillForge.sln ./

# Copy project files
COPY src/Core/Domain/Domain.csproj src/Core/Domain/
COPY src/Core/Application/Application.csproj src/Core/Application/
COPY src/Infrastructure/Persistence/Persistence.csproj src/Infrastructure/Persistence/
COPY src/Presentation/WebApi/WebApi.csproj src/Presentation/WebApi/
COPY tests/Application.UnitTests/Application.UnitTests.csproj tests/Application.UnitTests/
COPY tests/Application.IntegrationTests/Application.IntegrationTests.csproj tests/Application.IntegrationTests/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src/src/Presentation/WebApi
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish --no-restore

# Use the official .NET 8 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create uploads directory for file uploads
RUN mkdir -p wwwroot/uploads

# Copy the published application
COPY --from=build /app/publish .

# Expose the port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "WebApi.dll"]