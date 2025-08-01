using System.Text;
using System.Text.Json;
using Application.Common;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace WebApi.Middleware;

public class ResponseValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseValidationMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ResponseValidationMiddleware(RequestDelegate next, ILogger<ResponseValidationMiddleware> logger, IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only validate in development environment
        if (!_environment.IsDevelopment())
        {
            await _next(context);
            return;
        }

        var originalBodyStream = context.Response.Body;

        try
        {
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

            // Skip validation for non-JSON responses or empty responses
            if (string.IsNullOrWhiteSpace(responseBody) || 
                !IsJsonResponse(context.Response.ContentType))
            {
                await WriteResponseToOriginalStream(responseBody, responseBodyStream, originalBodyStream);
                return;
            }

            try
            {
                ValidateResponseStructure(context.Request.Path, context.Request.Method, responseBody, context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Response validation failed for {Method} {Path}. Response: {Response}", 
                    context.Request.Method, context.Request.Path, responseBody);
            }

            await WriteResponseToOriginalStream(responseBody, responseBodyStream, originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private static bool IsJsonResponse(string? contentType)
    {
        return contentType?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true;
    }

    private static async Task WriteResponseToOriginalStream(string responseBody, MemoryStream responseBodyStream, Stream originalBodyStream)
    {
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        await responseBodyStream.CopyToAsync(originalBodyStream);
    }

    private void ValidateResponseStructure(string path, string method, string responseBody, int statusCode)
    {
        try
        {
            // Parse the response as JSON
            var jsonResponse = JObject.Parse(responseBody);
            
            // Get the appropriate schema for this endpoint
            string schemaJson = GetSchemaForResponse(path, method, statusCode);
            
            if (string.IsNullOrEmpty(schemaJson))
            {
                _logger.LogDebug("No schema defined for {Method} {Path}", method, path);
                return;
            }

            // Validate against schema
            var schema = JSchema.Parse(schemaJson);
            bool isValid = jsonResponse.IsValid(schema, out IList<string> errorMessages);

            if (!isValid)
            {
                _logger.LogWarning("Response validation failed for {Method} {Path}. Errors: {Errors}", 
                    method, path, string.Join(", ", errorMessages));
            }
            else
            {
                _logger.LogDebug("Response validation passed for {Method} {Path}", method, path);
            }
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Failed to parse JSON response for validation: {Method} {Path}", method, path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during response validation: {Method} {Path}", method, path);
        }
    }

    private static string GetSchemaForResponse(string path, string method, int statusCode)
    {
        // For successful responses, use endpoint-specific schemas
        if (statusCode >= 200 && statusCode < 300)
        {
            return ApiContracts.GetSchemaForEndpoint(path, method);
        }

        // For error responses, expect a standard error format
        if (statusCode >= 400)
        {
            return """
            {
                "oneOf": [
                    {
                        "type": "object",
                        "properties": {
                            "success": {"type": "boolean", "const": false},
                            "data": {},
                            "message": {"type": "string"},
                            "errors": {
                                "type": "array",
                                "items": {"type": "string"}
                            },
                            "timestamp": {"type": "string", "format": "date-time"}
                        },
                        "required": ["success", "message", "errors", "timestamp"],
                        "additionalProperties": false
                    },
                    {
                        "type": "string"
                    }
                ]
            }
            """;
        }

        return string.Empty;
    }
}