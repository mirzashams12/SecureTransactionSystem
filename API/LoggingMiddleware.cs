public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request is for the blocked endpoint
        // This demonstrates how middleware can be used to intercept and handle specific requests, allowing you to implement custom logic such as blocking certain endpoints or modifying the request/response. In this case, if the request path starts with "/api/test/blocked", the middleware will return a message indicating that the endpoint is blocked and will not call the next middleware in the pipeline, effectively preventing further processing of that request.
        if (context.Request.Path.StartsWithSegments("/api/test/blocked"))
        {
            await context.Response.WriteAsync("This endpoint is blocked and will not be processed.");
            return;
        }

        // Log the incoming request
        Console.WriteLine($"Incoming request: {context.Request.Method} {context.Request.Path}");

        // Call the next middleware in the pipeline
        await _next(context);

        // Log the outgoing response
        Console.WriteLine($"Outgoing response: {context.Response.StatusCode}");
    }
}