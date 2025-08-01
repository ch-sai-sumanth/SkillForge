namespace Application.Common;

public static class ApiContracts
{
    public static readonly string AuthResponseSchema = """
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

    public static readonly string UserDtoSchema = """
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
        "additionalProperties": false
    }
    """;

    public static readonly string SessionDtoSchema = """
    {
        "type": "object",
        "properties": {
            "id": {"type": "string"},
            "mentorId": {"type": "string", "minLength": 1},
            "menteeId": {"type": "string", "minLength": 1},
            "scheduledDate": {"type": "string", "format": "date-time"},
            "duration": {"type": "integer", "minimum": 1},
            "status": {"type": "string", "enum": ["Pending", "Accepted", "Declined", "Completed", "Cancelled"]},
            "topic": {"type": "string"},
            "notes": {"type": "string"}
        },
        "required": ["mentorId", "menteeId", "scheduledDate", "duration", "status"],
        "additionalProperties": false
    }
    """;

    public static readonly string PaymentDtoSchema = """
    {
        "type": "object",
        "properties": {
            "id": {"type": "string"},
            "userId": {"type": "string", "minLength": 1},
            "subscriptionId": {"type": "string", "minLength": 1},
            "amount": {"type": "number", "minimum": 0},
            "paymentDate": {"type": "string", "format": "date-time"},
            "status": {"type": "string", "enum": ["Pending", "Completed", "Failed", "Refunded"]},
            "transactionId": {"type": "string"}
        },
        "required": ["userId", "subscriptionId", "amount", "paymentDate", "status"],
        "additionalProperties": false
    }
    """;

    public static readonly string SubscriptionPlanDtoSchema = """
    {
        "type": "object",
        "properties": {
            "id": {"type": "string"},
            "name": {"type": "string", "minLength": 1},
            "description": {"type": "string"},
            "price": {"type": "number", "minimum": 0},
            "durationInDays": {"type": "integer", "minimum": 1},
            "features": {
                "type": "array",
                "items": {"type": "string"}
            }
        },
        "required": ["name", "price", "durationInDays"],
        "additionalProperties": false
    }
    """;

    public static readonly string GoalDtoSchema = """
    {
        "type": "object",
        "properties": {
            "id": {"type": "string"},
            "menteeId": {"type": "string", "minLength": 1},
            "title": {"type": "string", "minLength": 1},
            "description": {"type": "string"},
            "targetDate": {"type": "string", "format": "date-time"},
            "isCompleted": {"type": "boolean"}
        },
        "required": ["menteeId", "title", "targetDate"],
        "additionalProperties": false
    }
    """;

    public static readonly string TaskDtoSchema = """
    {
        "type": "object",
        "properties": {
            "id": {"type": "string"},
            "title": {"type": "string", "minLength": 1},
            "description": {"type": "string"},
            "assignedTo": {"type": "string", "minLength": 1},
            "assignedBy": {"type": "string", "minLength": 1},
            "dueDate": {"type": "string", "format": "date-time"},
            "isCompleted": {"type": "boolean"},
            "priority": {"type": "string", "enum": ["Low", "Medium", "High"]}
        },
        "required": ["title", "assignedTo", "assignedBy", "dueDate"],
        "additionalProperties": false
    }
    """;

    public static readonly string ApiResponseSchema = """
    {
        "type": "object",
        "properties": {
            "success": {"type": "boolean"},
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
    }
    """;

    public static string GetSchemaForEndpoint(string endpoint, string method)
    {
        return endpoint.ToLower() switch
        {
            "/api/auth/login" when method == "POST" => AuthResponseSchema,
            "/api/auth/refresh-token" when method == "POST" => AuthResponseSchema,
            "/api/user" when method == "GET" => CreateArraySchema(UserDtoSchema),
            "/api/session" when method == "GET" => CreateArraySchema(SessionDtoSchema),
            "/api/payment" when method == "GET" => CreateArraySchema(PaymentDtoSchema),
            "/api/subscriptionplan" when method == "GET" => CreateArraySchema(SubscriptionPlanDtoSchema),
            "/api/goal" when method == "GET" => CreateArraySchema(GoalDtoSchema),
            "/api/task" when method == "GET" => CreateArraySchema(TaskDtoSchema),
            _ => ApiResponseSchema
        };
    }

    private static string CreateArraySchema(string itemSchema)
    {
        return $$"""
        {
            "type": "array",
            "items": {{itemSchema}}
        }
        """;
    }
}