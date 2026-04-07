using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using static System.Net.Mime.MediaTypeNames;

namespace webapi.middlewares;
// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class RequesLoggingMiddleware
{
  private readonly RequestDelegate _next;

  public RequesLoggingMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext httpContext)
  {
    var path = httpContext.Request.Path;
    Console.WriteLine($"Request: {path}");

    await _next(httpContext);

    Console.WriteLine($"Response: {httpContext.Response.StatusCode}");
  }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class RequesLoggingMiddlewareExtensions
{
  public static IApplicationBuilder UseRequesLoggingMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<RequesLoggingMiddleware>();
  }
}
