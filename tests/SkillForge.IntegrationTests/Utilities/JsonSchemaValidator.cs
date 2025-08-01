using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using FluentAssertions;

namespace SkillForge.IntegrationTests.Utilities;

public static class JsonSchemaValidator
{
    public static void ValidateSchema(string jsonContent, string schemaJson, string context = "")
    {
        try
        {
            var jsonObject = JToken.Parse(jsonContent);
            var schema = JSchema.Parse(schemaJson);
            
            bool isValid = jsonObject.IsValid(schema, out IList<string> errorMessages);
            
            if (!isValid)
            {
                var errors = string.Join(Environment.NewLine, errorMessages);
                var contextInfo = string.IsNullOrEmpty(context) ? "" : $" in {context}";
                throw new JsonSchemaValidationException($"JSON Schema validation failed{contextInfo}:{Environment.NewLine}{errors}{Environment.NewLine}JSON:{Environment.NewLine}{jsonContent}");
            }
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            throw new JsonSchemaValidationException($"Invalid JSON format: {ex.Message}", ex);
        }
    }

    public static void ValidateAuthResponse(string jsonContent)
    {
        const string schema = """
        {
            "type": "object",
            "properties": {
                "token": {"type": "string", "minLength": 1},
                "refreshToken": {"type": "string", "minLength": 1}
            },
            "required": ["token", "refreshToken"],
            "additionalProperties": false
        }
        """;
        
        ValidateSchema(jsonContent, schema, "Auth Response");
    }

    public static void ValidateUserDto(string jsonContent)
    {
        const string schema = """
        {
            "type": "object",
            "properties": {
                "id": {"type": "string"},
                "name": {"type": "string", "minLength": 1},
                "username": {"type": "string", "minLength": 1},
                "email": {"type": "string", "format": "email"},
                "role": {"type": "string", "enum": ["Admin", "Mentor", "Mentee"]},
                "skills": {
                    "type": "array",
                    "items": {"type": "string"}
                }
            },
            "required": ["name", "username", "email", "role"],
            "additionalProperties": true
        }
        """;
        
        ValidateSchema(jsonContent, schema, "User DTO");
    }

    public static void ValidateUserDtoArray(string jsonContent)
    {
        const string schema = """
        {
            "type": "array",
            "items": {
                "type": "object",
                "properties": {
                    "id": {"type": "string"},
                    "name": {"type": "string", "minLength": 1},
                    "username": {"type": "string", "minLength": 1},
                    "email": {"type": "string", "format": "email"},
                    "role": {"type": "string", "enum": ["Admin", "Mentor", "Mentee"]},
                    "skills": {
                        "type": "array",
                        "items": {"type": "string"}
                    }
                },
                "required": ["name", "username", "email", "role"],
                "additionalProperties": true
            }
        }
        """;
        
        ValidateSchema(jsonContent, schema, "User DTO Array");
    }

    public static void ValidateSessionDto(string jsonContent)
    {
        const string schema = """
        {
            "type": "object",
            "properties": {
                "id": {"type": "string"},
                "mentorId": {"type": "string", "minLength": 1},
                "menteeId": {"type": "string", "minLength": 1},
                "scheduledDate": {"type": "string"},
                "duration": {"type": "integer", "minimum": 1},
                "status": {"type": "string", "enum": ["Pending", "Accepted", "Declined", "Completed", "Cancelled"]},
                "topic": {"type": "string"},
                "notes": {"type": "string"}
            },
            "required": ["mentorId", "menteeId", "scheduledDate", "duration", "status"],
            "additionalProperties": true
        }
        """;
        
        ValidateSchema(jsonContent, schema, "Session DTO");
    }

    public static void ValidateErrorResponse(string jsonContent)
    {
        // Allow both structured error responses and simple string responses
        const string schema = """
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
                        "timestamp": {"type": "string"}
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
        
        ValidateSchema(jsonContent, schema, "Error Response");
    }

    public static void ValidateApiResponse<T>(string jsonContent, string dataSchema)
    {
        var schema = $$"""
        {
            "type": "object",
            "properties": {
                "success": {"type": "boolean"},
                "data": {{dataSchema}},
                "message": {"type": "string"},
                "errors": {
                    "type": "array",
                    "items": {"type": "string"}
                },
                "timestamp": {"type": "string"}
            },
            "required": ["success", "message", "errors", "timestamp"],
            "additionalProperties": false
        }
        """;
        
        ValidateSchema(jsonContent, schema, "API Response");
    }
}

public class JsonSchemaValidationException : Exception
{
    public JsonSchemaValidationException(string message) : base(message) { }
    public JsonSchemaValidationException(string message, Exception innerException) : base(message, innerException) { }
}