using System.Text;
using System.Text.Json;


namespace AgentManagementAPI.Middlewares.Global;

public class GlobalLoggingMiddleware
{

    private readonly RequestDelegate _next;

    public GlobalLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        Console.WriteLine($"Got Request to server: {request.Method} {request.Path}\n" +
                          $"From IP: {request.HttpContext.Connection.RemoteIpAddress}");
        await _next(context);
    }
}


