using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace webapi.middlewares;
// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class BasicAuthMiddleware
{
  private readonly RequestDelegate _next;
  private readonly string _user;
  private readonly string _password;

  public BasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
  {
    _next = next;
    _user = configuration["BasicAuth:Username"] ?? string.Empty;
    _password = configuration["BasicAuth:Password"] ?? string.Empty;
  }

  public async Task Invoke(HttpContext context)
  {
    var path = context.Request.Path.Value ?? string.Empty;
    // Omitir Swagger/Scalar/OpenAPI para poder usar la documentación
    if (path.Contains("swagger", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("scalar", StringComparison.OrdinalIgnoreCase) ||
        path.Contains("openapi", StringComparison.OrdinalIgnoreCase))
    {
      await _next(context);
      return;
    }

    if (!context.Request.Headers.TryGetValue("Authorization", out var header) ||
        !header.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      return;
    }

    var token = header.ToString().Substring("Basic ".Length).Trim();
    var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token));
    var parts = decoded.Split(':', 2);
    var user = parts.ElementAtOrDefault(0);
    var pass = parts.ElementAtOrDefault(1);

    if (user == _user && pass == _password)
      await _next(context);
    else
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
  }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class BasicAuthMiddlewareExtensions
{
  public static IApplicationBuilder UseBasicAuthMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<BasicAuthMiddleware>();
  }
}
