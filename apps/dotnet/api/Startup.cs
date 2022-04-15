using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solutions.Dotnet.Core.Interfaces;
using Solutions.Dotnet.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

// ! We need this here, .NET 3.1 or .NET 5 style
namespace Solutions.Dotnet.API;
public class Startup
{
  private readonly IConfiguration _config;
  public Startup(IConfiguration config)
  {
    _config = config;
  }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    // We add our services here so we can add them to DI, order here does not matter
    services.AddScoped<IProductRepository, ProductRepository>();

    services.AddControllers();

    services.AddDbContext<StoreContext>(x => x.UseSqlite(this._config.GetConnectionString("DefaultConnection")));

    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    });
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // order of operations here does matter
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}
