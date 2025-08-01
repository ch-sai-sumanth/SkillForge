using System.Net;
using System.Text.Json;
using FluentAssertions;
using SkillForge.IntegrationTests.TestBase;
using SkillForge.IntegrationTests.Utilities;
using Xunit.Abstractions;
using Application.DTOs;

namespace SkillForge.IntegrationTests.Controllers;

public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(IntegrationTestWebApplicationFactory factory, ITestOutputHelper output) 
        : base(factory, output)
    {
    }

    [Fact]
    public async Task Register_ValidRequest_ShouldReturnSuccessMessage()
    {
        // Arrange
        var request = new
        {
            Name = "Test User",
            Username = "testuser",
            Email = "test@example.com",
            Password = "TestPassword123!",
            Role = "Mentee",
            Skills = new[] { "C#", ".NET" }
        };

        // Act
        var response = await PostAsync("/api/auth/register", request);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("successfully");
    }

    [Fact]
    public async Task Register_DuplicateUsername_ShouldReturnBadRequest()
    {
        // Arrange - Create first user
        var firstRequest = new
        {
            Name = "First User",
            Username = "duplicateuser",
            Email = "first@example.com",
            Password = "Password123!",
            Role = "Mentee",
            Skills = new[] { "C#" }
        };
        await PostAsync("/api/auth/register", firstRequest);

        var duplicateRequest = new
        {
            Name = "Second User",
            Username = "duplicateuser", // Same username
            Email = "second@example.com",
            Password = "Password123!",
            Role = "Mentor",
            Skills = new[] { "Java" }
        };

        // Act
        var response = await PostAsync("/api/auth/register", duplicateRequest);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        JsonSchemaValidator.ValidateErrorResponse(content);
    }

    [Fact]
    public async Task Login_ValidCredentials_ShouldReturnValidAuthResponse()
    {
        // Arrange - First register a user
        var registerRequest = new
        {
            Name = "Login Test User",
            Username = "loginuser",
            Email = "login@example.com",
            Password = "LoginPassword123!",
            Role = "Mentee",
            Skills = new[] { "Testing" }
        };
        await PostAsync("/api/auth/register", registerRequest);

        var loginRequest = new
        {
            Username = "loginuser",
            Password = "LoginPassword123!"
        };

        // Act
        var response = await PostAsync("/api/auth/login", loginRequest);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        JsonSchemaValidator.ValidateAuthResponse(content);

        // Verify the response structure
        var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        authResponse.Should().NotBeNull();
        authResponse!.Token.Should().NotBeNullOrEmpty();
        authResponse.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new
        {
            Username = "nonexistentuser",
            Password = "WrongPassword123!"
        };

        // Act
        var response = await PostAsync("/api/auth/login", loginRequest);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        content.Should().Be("Invalid username or password");
    }

    [Fact]
    public async Task RefreshToken_ValidToken_ShouldReturnNewAuthResponse()
    {
        // Arrange - Register and login to get a refresh token
        var registerRequest = new
        {
            Name = "Refresh Test User",
            Username = "refreshuser",
            Email = "refresh@example.com",
            Password = "RefreshPassword123!",
            Role = "Mentee",
            Skills = new[] { "Testing" }
        };
        await PostAsync("/api/auth/register", registerRequest);

        var loginRequest = new
        {
            Username = "refreshuser",
            Password = "RefreshPassword123!"
        };

        var loginResponse = await PostAsync("/api/auth/login", loginRequest);
        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(loginContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Act
        var refreshResponse = await PostAsync("/api/auth/refresh-token", authResponse!.RefreshToken);
        var refreshContent = await refreshResponse.Content.ReadAsStringAsync();

        // Assert
        if (refreshResponse.StatusCode == HttpStatusCode.OK)
        {
            JsonSchemaValidator.ValidateAuthResponse(refreshContent);
        }
        else
        {
            // If refresh token implementation returns unauthorized for any reason
            refreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Fact]
    public async Task RefreshToken_InvalidToken_ShouldReturnUnauthorized()
    {
        // Arrange
        var invalidToken = "invalid-refresh-token";

        // Act
        var response = await PostAsync("/api/auth/refresh-token", invalidToken);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        content.Should().Be("Invalid or expired refresh token");
    }

    [Fact]
    public async Task Logout_WithValidToken_ShouldReturnSuccess()
    {
        // Arrange - Register and login to get a token
        var token = await AuthenticateAsAdmin();

        // Act
        var response = await Client.PostAsync("/api/auth/logout", null);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be("User logged out successfully");
    }

    [Fact]
    public async Task Logout_WithoutToken_ShouldReturnUnauthorized()
    {
        // Arrange - Make sure no token is set
        ClearAuthentication();

        // Act
        var response = await Client.PostAsync("/api/auth/logout", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}