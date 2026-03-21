public class LearnMiddleware
{
    private readonly RequestDelegate _next;

    public LearnMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// This method demonstrates how to implement custom middleware in an ASP.NET Core application. Middleware is a component that can inspect, modify, or short-circuit HTTP requests and responses as they pass through the pipeline. In this example, the middleware logs the incoming request before calling the next middleware in the pipeline and logs the outgoing response after the next middleware has processed the request. This allows you to add cross-cutting concerns such as logging, authentication, or error handling in a centralized manner without modifying individual controllers or actions.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Log the incoming request
        Console.WriteLine("Before next");

        // Call the next middleware in the pipeline
        await _next(context);

        // Log the outgoing response
        Console.WriteLine("After next");
    }
}