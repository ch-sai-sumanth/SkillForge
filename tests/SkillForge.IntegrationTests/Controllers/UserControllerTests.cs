using System.Net;
using System.Text.Json;
using FluentAssertions;
using SkillForge.IntegrationTests.TestBase;
using SkillForge.IntegrationTests.Utilities;
using Xunit.Abstractions;
using Application.DTOs;

namespace SkillForge.IntegrationTests.Controllers;

public class UserControllerTests : IntegrationTestBase
{
    public UserControllerTests(IntegrationTestWebApplicationFactory factory, ITestOutputHelper output) 
        : base(factory, output)
    {
    }

    [Fact]
    public async Task GetAll_WithAdminToken_ShouldReturnValidUserArray()
    {
        // Arrange
        await AuthenticateAsAdmin();

        // Act
        var response = await GetAsync("/api/user");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        JsonSchemaValidator.ValidateUserDtoArray(content);

        // Verify the response contains at least the admin user we created
        var users = JsonSerializer.Deserialize<UserDto[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        users.Should().NotBeNull();
        users!.Should().NotBeEmpty();
        users.Should().Contain(u => u.Role == "Admin");
    }

    [Fact]
    public async Task GetAll_WithoutAdminToken_ShouldReturnUnauthorized()
    {
        // Arrange - Clear any existing authentication
        ClearAuthentication();

        // Act
        var response = await GetAsync("/api/user");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_ExistingUser_ShouldReturnValidUserDto()
    {
        // Arrange
        await AuthenticateAsAdmin();
        
        // First get all users to find a valid ID
        var allUsersResponse = await GetAsync("/api/user");
        var allUsersContent = await allUsersResponse.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<UserDto[]>(allUsersContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var testUserId = users!.First().Id;

        // Act
        var response = await GetAsync($"/api/user/{testUserId}");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        JsonSchemaValidator.ValidateUserDto(content);

        var user = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        user.Should().NotBeNull();
        user!.Id.Should().Be(testUserId);
    }

    [Fact]
    public async Task GetById_NonExistentUser_ShouldReturnNotFound()
    {
        // Arrange
        await AuthenticateAsAdmin();
        var nonExistentId = "507f1f77bcf86cd799439011"; // Valid ObjectId format but non-existent

        // Act
        var response = await GetAsync($"/api/user/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ValidUserDto_ShouldReturnCreated()
    {
        // Arrange
        await AuthenticateAsAdmin();
        
        var newUser = new
        {
            Name = "New Test User",
            Username = "newtestuser",
            Email = "newuser@test.com",
            Role = "Mentee",
            Skills = new[] { "JavaScript", "React" }
        };

        // Act
        var response = await PostAsync("/api/user", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_WithoutAdminToken_ShouldReturnUnauthorized()
    {
        // Arrange
        ClearAuthentication();
        
        var newUser = new
        {
            Name = "Unauthorized User",
            Username = "unauthorizeduser",
            Email = "unauthorized@test.com",
            Role = "Mentee",
            Skills = new[] { "Testing" }
        };

        // Act
        var response = await PostAsync("/api/user", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Update_ExistingUser_ShouldReturnNoContent()
    {
        // Arrange
        await AuthenticateAsAdmin();
        
        // First create a user to update
        var createUser = new
        {
            Name = "User To Update",
            Username = "usertoupdate",
            Email = "update@test.com",
            Role = "Mentee",
            Skills = new[] { "Python" }
        };
        
        await PostAsync("/api/user", createUser);
        
        // Get all users to find the ID of the user we just created
        var allUsersResponse = await GetAsync("/api/user");
        var allUsersContent = await allUsersResponse.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<UserDto[]>(allUsersContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        var userToUpdate = users!.First(u => u.Username == "usertoupdate");
        
        var updateData = new
        {
            Name = "Updated User Name",
            Username = "usertoupdate",
            Email = "updated@test.com",
            Role = "Mentor",
            Skills = new[] { "Python", "Django" }
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/api/user/{userToUpdate.Id}", updateData);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ExistingUser_ShouldReturnNoContent()
    {
        // Arrange
        await AuthenticateAsAdmin();
        
        // First create a user to delete
        var createUser = new
        {
            Name = "User To Delete",
            Username = "usertodelete",
            Email = "delete@test.com",
            Role = "Mentee",
            Skills = new[] { "JavaScript" }
        };
        
        await PostAsync("/api/user", createUser);
        
        // Get all users to find the ID of the user we just created
        var allUsersResponse = await GetAsync("/api/user");
        var allUsersContent = await allUsersResponse.Content.ReadAsStringAsync();
        var users = JsonSerializer.Deserialize<UserDto[]>(allUsersContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        var userToDelete = users!.First(u => u.Username == "usertodelete");

        // Act
        var response = await Client.DeleteAsync($"/api/user/{userToDelete.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Verify the user is actually deleted
        var verifyResponse = await GetAsync($"/api/user/{userToDelete.Id}");
        verifyResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}