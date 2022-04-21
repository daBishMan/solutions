
namespace Solutions.Dotnet.API.Extensions;

public static class SwaggerServiceExtensions
{
  public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
  {
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    return services;
  }

  public static WebApplication UseSwaggerDocumentation(this WebApplication app)
  {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Solutions.Dotnet.API V1");
    });

    return app;
  }
}
