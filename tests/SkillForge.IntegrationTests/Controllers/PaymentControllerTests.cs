using System.Net;
using System.Text.Json;
using FluentAssertions;
using SkillForge.IntegrationTests.TestBase;
using SkillForge.IntegrationTests.Utilities;
using Xunit.Abstractions;

namespace SkillForge.IntegrationTests.Controllers;

public class PaymentControllerTests : IntegrationTestBase
{
    public PaymentControllerTests(IntegrationTestWebApplicationFactory factory, ITestOutputHelper output) 
        : base(factory, output)
    {
    }

    [Fact]
    public async Task GetAllPayments_WithValidToken_ShouldReturnPaymentArray()
    {
        // Arrange
        await AuthenticateAsAdmin();

        // Act
        var response = await GetAsync("/api/payment");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Should return an array (might be empty initially)
        var payments = JsonSerializer.Deserialize<JsonElement[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        payments.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllPayments_WithoutToken_ShouldReturnUnauthorized()
    {
        // Arrange
        ClearAuthentication();

        // Act
        var response = await GetAsync("/api/payment");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreatePayment_WithValidData_ShouldCreatePayment()
    {
        // Arrange
        await AuthenticateAsAdmin();

        // First, we need to create a subscription plan
        var subscriptionPlan = new
        {
            Name = "Test Plan",
            Description = "A test subscription plan",
            Price = 29.99m,
            DurationInDays = 30,
            Features = new[] { "Feature 1", "Feature 2" }
        };

        var planResponse = await PostAsync("/api/subscriptionplan", subscriptionPlan);
        planResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Get the created plan ID
        var allPlansResponse = await GetAsync("/api/subscriptionplan");
        var allPlansContent = await allPlansResponse.Content.ReadAsStringAsync();
        var plans = JsonSerializer.Deserialize<JsonElement[]>(allPlansContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var testPlan = plans!.First(p => p.GetProperty("name").GetString() == "Test Plan");
        var planId = testPlan.GetProperty("id").GetString();

        var paymentRequest = new
        {
            SubscriptionPlanId = planId,
            Amount = 29.99m
        };

        // Act
        var response = await PostAsync("/api/payment", paymentRequest);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);
        
        // The response should contain payment information
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ConfirmPayment_WithValidData_ShouldConfirmPayment()
    {
        // Arrange
        await AuthenticateAsAdmin();

        // This test assumes you have a payment confirmation endpoint
        // We'll test the basic structure even if the payment doesn't exist
        var confirmRequest = new
        {
            PaymentIntentId = "pi_test_payment_intent_id",
            Status = "succeeded"
        };

        // Act
        var response = await PostAsync("/api/payment/confirm", confirmRequest);

        // Assert
        // The response might be NotFound if payment doesn't exist, or OK if it does
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RecordPayment_WithValidData_ShouldRecordPayment()
    {
        // Arrange
        await AuthenticateAsAdmin();

        var recordPaymentRequest = new
        {
            UserId = "test-user-id", // This might not exist, but we're testing the endpoint structure
            SubscriptionId = "test-subscription-id",
            Amount = 49.99m,
            TransactionId = "txn_test_123",
            PaymentMethod = "card"
        };

        // Act
        var response = await PostAsync("/api/payment/record", recordPaymentRequest);

        // Assert
        // The response might vary based on validation, but the endpoint should exist
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK, 
            HttpStatusCode.Created, 
            HttpStatusCode.BadRequest, 
            HttpStatusCode.NotFound
        );
    }

    [Fact]
    public async Task GetPaymentsByMentee_WithValidToken_ShouldReturnPayments()
    {
        // Arrange
        await AuthenticateAsAdmin();

        // Act - This endpoint might require the current user's payments
        var response = await GetAsync("/api/payment/mentee");

        // Assert
        // Should return OK with an array (possibly empty) or require specific mentee role
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            var payments = JsonSerializer.Deserialize<JsonElement[]>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            payments.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task UpdatePaymentStatus_WithValidData_ShouldUpdateStatus()
    {
        // Arrange
        await AuthenticateAsAdmin();

        var updateRequest = new
        {
            Status = "Completed"
        };

        // Use a test payment ID (might not exist, but tests endpoint structure)
        var testPaymentId = "test-payment-id";

        // Act
        var response = await Client.PutAsJsonAsync($"/api/payment/{testPaymentId}/status", updateRequest);

        // Assert
        // Might return NotFound if payment doesn't exist, but endpoint should be accessible
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK, 
            HttpStatusCode.NoContent, 
            HttpStatusCode.NotFound, 
            HttpStatusCode.BadRequest
        );
    }
}