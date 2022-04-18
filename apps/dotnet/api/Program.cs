using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Solutions.Dotnet.API.Errors;
using Solutions.Dotnet.API.Helpers;
using Solutions.Dotnet.API.Middleware;
using Solutions.Dotnet.Core.Interfaces;
using Solutions.Dotnet.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// We add our services here so we can add them to DI, order here does not matter
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(x =>
  x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// custom error handling for validation errors
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
  options.InvalidModelStateResponseFactory = actionContext =>
  {
    var errors = actionContext.ModelState
      .Where(e => e.Value.Errors.Count > 0)
      .SelectMany(x => x.Value.Errors)
      .Select(x => x.ErrorMessage).ToArray();

    var errorResponse = new ApiValidationErrorResponse { Errors = errors };

    return new BadRequestObjectResult(errorResponse);
  };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

// if (app.Environment.IsDevelopment())
// {
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Solutions.Dotnet.API V1");
  });
// }

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// To Seed the Data
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  var loggerFactory = services.GetRequiredService<ILoggerFactory>();
  try
  {
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, loggerFactory);
  }
  catch (Exception ex)
  {
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred during migration");
  }
}

app.Run();
