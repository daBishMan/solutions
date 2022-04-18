using System.Net;
using System.Text.Json;
using Solutions.Dotnet.API.Errors;

namespace Solutions.Dotnet.API.Middleware;

public class ExceptionMiddleware
{
  private readonly RequestDelegate next;
  private readonly ILogger<ExceptionMiddleware> logger;
  private readonly IHostEnvironment env;

  public ExceptionMiddleware(
    RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
  {
    this.next = next;
    this.logger = logger;
    this.env = env;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await this.next(httpContext);
    }
    catch (Exception ex)
    {
      this.logger.LogError(ex, ex.Message);

      httpContext.Response.ContentType = "application/json";
      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      var response = this.env.IsDevelopment()
        ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
        : new ApiException((int)HttpStatusCode.InternalServerError);

      var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
      var json = JsonSerializer.Serialize(response, options);

      await httpContext.Response.WriteAsync(json);
    }
  }
}
