# API Response Validation Guide

## Overview

This validation system ensures your existing API contracts remain stable during DDD refactoring. It provides comprehensive testing and validation of API responses to prevent breaking changes.

## Components

### 1. API Response Wrapper (`ApiResponse<T>`)

Standardized response format for all API endpoints:

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public DateTime Timestamp { get; set; }
}
```

### 2. JSON Schema Contracts (`ApiContracts`)

Predefined schemas for validating response structures:
- AuthResponse validation
- UserDto validation  
- SessionDto validation
- PaymentDto validation
- And more...

### 3. Response Validation Middleware

**Development only** - Validates responses against predefined schemas:
- Automatic JSON schema validation
- Logs validation failures
- Does not affect production performance

### 4. Integration Test Framework

Comprehensive test suite covering:
- Authentication endpoints
- User management
- Payment processing
- Session handling

## Usage

### Running Validation Tests

```bash
# Make the script executable (first time only)
chmod +x run-validation-tests.sh

# Run all validation tests
./run-validation-tests.sh
```

### Before DDD Refactoring

1. **Establish Baseline**: Run tests to capture current API behavior
   ```bash
   ./run-validation-tests.sh > baseline-results.txt
   ```

2. **Document Current Contracts**: Review `ApiContracts.cs` for your endpoint schemas

3. **Ensure All Tests Pass**: Fix any existing issues before refactoring

### During DDD Refactoring

1. **Run Tests Frequently**: After each significant change
   ```bash
   dotnet test tests/SkillForge.IntegrationTests/
   ```

2. **Check Validation Logs**: In development, check logs for schema validation warnings

3. **Update Tests Incrementally**: As you change internal structure, update test expectations if needed

### After DDD Refactoring

1. **Verify Contract Compliance**: All original tests should still pass
2. **Update Documentation**: Document any intentional API changes
3. **Performance Check**: Ensure no performance regression

## Using the New Response Format (Optional)

You can gradually migrate to the standardized response format:

```csharp
// Old way
return Ok(userData);

// New way with helper extensions
return this.ApiSuccess(userData, "User retrieved successfully");

// Error responses
return this.ApiError("User not found", statusCode: 404);
```

### Controller Extensions Available

```csharp
this.ApiSuccess(data, message)           // 200 OK with data
this.ApiSuccess(message)                 // 200 OK with message only
this.ApiError(message, errors, statusCode) // Error with custom status
this.ApiNotFound(message)               // 404 Not Found
this.ApiUnauthorized(message)           // 401 Unauthorized
this.ApiForbidden(message)              // 403 Forbidden
this.ApiValidationError(errors)         // 400 Bad Request with validation errors
```

## Test Structure

### Integration Tests

Located in `tests/SkillForge.IntegrationTests/`:

- **TestBase/**: Base classes and test infrastructure
- **Controllers/**: Endpoint-specific test suites
- **Utilities/**: JSON schema validation helpers

### Adding New Tests

```csharp
public class MyControllerTests : IntegrationTestBase
{
    public MyControllerTests(IntegrationTestWebApplicationFactory factory, ITestOutputHelper output) 
        : base(factory, output) { }

    [Fact]
    public async Task MyEndpoint_ShouldReturnValidResponse()
    {
        // Arrange
        await AuthenticateAsAdmin();
        
        // Act
        var response = await GetAsync("/api/my-endpoint");
        var content = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        JsonSchemaValidator.ValidateSchema(content, myExpectedSchema);
    }
}
```

## Configuration

### Middleware Registration

Already configured in `Program.cs`:
```csharp
app.UseMiddleware<ResponseValidationMiddleware>(); // Development only
```

### Schema Definitions

Update `ApiContracts.cs` to add new endpoint schemas or modify existing ones.

### Test Dependencies

Key NuGet packages:
- `Microsoft.AspNetCore.Mvc.Testing` - Integration testing
- `Newtonsoft.Json.Schema` - JSON schema validation
- `FluentAssertions` - Fluent test assertions
- `Testcontainers.MongoDb` - MongoDB test containers

## Troubleshooting

### Common Issues

1. **Docker Not Running**: Integration tests need Docker for MongoDB
2. **Schema Validation Failures**: Check the specific error messages for missing/incorrect properties
3. **Authentication Issues**: Ensure test user creation and authentication logic works
4. **Database Issues**: Verify MongoDB connection and test database setup

### Debugging

1. **Enable Detailed Logging**: Check development logs for validation middleware output
2. **Inspect Response Content**: Use test output to see actual vs expected responses  
3. **Schema Validation**: Use online JSON schema validators to debug schema issues

## Benefits

✅ **Prevent Breaking Changes**: Catch API contract violations early
✅ **Confidence in Refactoring**: Know that external interfaces remain stable
✅ **Automated Validation**: Continuous validation during development
✅ **Documentation**: Living documentation of your API contracts
✅ **Regression Testing**: Detect unintended changes immediately

## Best Practices

1. **Run Tests Before Each Commit**: Integrate into your development workflow
2. **Update Schemas Intentionally**: Only modify contracts when API changes are intended
3. **Test Different Scenarios**: Include success, error, and edge cases
4. **Keep Tests Fast**: Use test containers and parallel execution
5. **Version Your Contracts**: Consider API versioning for major changes

## Next Steps

1. Run baseline tests: `./run-validation-tests.sh`
2. Start your DDD refactoring with confidence!
3. Consider gradually migrating to the standardized `ApiResponse<T>` format
4. Add more endpoint-specific tests as needed
5. Integrate validation tests into your CI/CD pipeline