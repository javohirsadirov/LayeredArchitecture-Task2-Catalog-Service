namespace CatalogService.Middlewares;

/// <summary>
/// Middleware that logs the Authorization token from incoming requests.
/// </summary>
public class TokenLoggingMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Invokes the middleware to log the token and pass the request to the next middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"TOKEN: {token}");
        }

        await next(context);
    }
}
