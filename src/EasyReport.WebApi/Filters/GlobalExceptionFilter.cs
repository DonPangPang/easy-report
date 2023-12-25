using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace EasyReport.WebApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var response = context.HttpContext.Response;
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { message = exception?.Message });
        response.WriteAsync(result);
    }
}