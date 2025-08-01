using Microsoft.AspNetCore.Mvc;
using Application.Common;

namespace WebApi.Extensions;

public static class ControllerExtensions
{
    public static IActionResult ApiSuccess<T>(this ControllerBase controller, T data, string message = "")
    {
        var response = ApiResponse<T>.SuccessResult(data, message);
        return controller.Ok(response);
    }

    public static IActionResult ApiSuccess(this ControllerBase controller, string message = "Operation completed successfully")
    {
        var response = ApiResponse.Success(message);
        return controller.Ok(response);
    }

    public static IActionResult ApiError(this ControllerBase controller, string message, List<string>? errors = null, int statusCode = 400)
    {
        var response = ApiResponse.Error(message, errors);
        return controller.StatusCode(statusCode, response);
    }

    public static IActionResult ApiError<T>(this ControllerBase controller, string message, List<string>? errors = null, int statusCode = 400)
    {
        var response = ApiResponse<T>.ErrorResult(message, errors);
        return controller.StatusCode(statusCode, response);
    }

    public static IActionResult ApiValidationError(this ControllerBase controller, List<string> errors)
    {
        var response = ApiResponse.Error("Validation failed", errors);
        return controller.BadRequest(response);
    }

    public static IActionResult ApiNotFound(this ControllerBase controller, string message = "Resource not found")
    {
        var response = ApiResponse.Error(message);
        return controller.NotFound(response);
    }

    public static IActionResult ApiUnauthorized(this ControllerBase controller, string message = "Unauthorized access")
    {
        var response = ApiResponse.Error(message);
        return controller.Unauthorized(response);
    }

    public static IActionResult ApiForbidden(this ControllerBase controller, string message = "Access forbidden")
    {
        var response = ApiResponse.Error(message);
        return controller.StatusCode(403, response);
    }
}