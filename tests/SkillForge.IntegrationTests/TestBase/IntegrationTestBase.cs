using System.Text;
using System.Text.Json;
using Application.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Testcontainers.MongoDb;
using Xunit.Abstractions;

namespace SkillForge.IntegrationTests.TestBase;

public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebApplicationFactory>, IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly IntegrationTestWebApplicationFactory Factory;
    protected readonly ITestOutputHelper Output;
    private readonly MongoDbContainer _mongoContainer;

    protected IntegrationTestBase(IntegrationTestWebApplicationFactory factory, ITestOutputHelper output)
    {
        Factory = factory;
        Output = output;
        
        _mongoContainer = new MongoDbBuilder()
            .WithImage("mongo:7.0")
            .WithPortBinding(27017)
            .Build();

        Client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Override MongoDB connection string for testing
                services.Configure<Infrastructure.Configuration.MongoDbSettings>(options =>
                {
                    options.ConnectionString = _mongoContainer.GetConnectionString();
                    options.DatabaseName = "SkillForgeTestDb";
                });
            });
            
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddXUnit(output);
            });
        }).CreateClient();
    }

    public async Task InitializeAsync()
    {
        await _mongoContainer.StartAsync();
        
        // Wait for the database to be ready
        await Task.Delay(2000);
    }

    public async Task DisposeAsync()
    {
        await _mongoContainer.StopAsync();
        Client.Dispose();
    }

    protected async Task<T> PostAsync<T>(string endpoint, object request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await Client.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Output.WriteLine($"POST {endpoint}: {response.StatusCode}");
        Output.WriteLine($"Response: {responseContent}");
        
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    protected async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Output.WriteLine($"GET {endpoint}: {response.StatusCode}");
        Output.WriteLine($"Response: {responseContent}");
        
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    protected async Task<HttpResponseMessage> PostAsync(string endpoint, object request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await Client.PostAsync(endpoint, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Output.WriteLine($"POST {endpoint}: {response.StatusCode}");
        Output.WriteLine($"Response: {responseContent}");
        
        return response;
    }

    protected async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        var response = await Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Output.WriteLine($"GET {endpoint}: {response.StatusCode}");
        Output.WriteLine($"Response: {responseContent}");
        
        return response;
    }

    protected void ValidateJsonSchema(string jsonContent, string schemaJson)
    {
        try
        {
            var jsonObject = JObject.Parse(jsonContent);
            var schema = JSchema.Parse(schemaJson);
            
            bool isValid = jsonObject.IsValid(schema, out IList<string> errorMessages);
            
            if (!isValid)
            {
                var errors = string.Join(", ", errorMessages);
                throw new JsonSchemaValidationException($"JSON Schema validation failed: {errors}");
            }
        }
        catch (JsonException ex)
        {
            throw new JsonSchemaValidationException($"Invalid JSON format: {ex.Message}", ex);
        }
    }

    protected async Task<string> AuthenticateAsAdmin()
    {
        // First register an admin user
        var registerRequest = new
        {
            Name = "Test Admin",
            Username = "testadmin",
            Email = "admin@test.com",
            Password = "AdminPassword123!",
            Role = "Admin",
            Skills = new[] { "Management" }
        };

        await PostAsync("/api/auth/register", registerRequest);

        // Then login
        var loginRequest = new
        {
            Username = "testadmin",
            Password = "AdminPassword123!"
        };

        var response = await PostAsync("/api/auth/login", loginRequest);
        var content = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var token = authResponse!.Token;
        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        return token;
    }

    protected void ClearAuthentication()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }
}

public class JsonSchemaValidationException : Exception
{
    public JsonSchemaValidationException(string message) : base(message) { }
    public JsonSchemaValidationException(string message, Exception innerException) : base(message, innerException) { }
}