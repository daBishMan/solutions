using Microsoft.EntityFrameworkCore;
using Solutions.Dotnet.Core.Interfaces;
using Solutions.Dotnet.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// We add our services here so we can add them to DI, order here does not matter
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(x =>
  x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

// ! We need this here, .NET 3.1 or .NET 5 style
// using Microsoft.EntityFrameworkCore;
// using Solutions.Dotnet.Infrastructure.Data;

// namespace Solutions.Dotnet.API;
// public class Program
// {
//   public static async Task Main(string[] args)
//   {
//     var host = CreateHostBuilder(args).Build();

//     using (var scope = host.Services.CreateScope())
//     {
//       var services = scope.ServiceProvider;
//       var loggerFactory = services.GetRequiredService<ILoggerFactory>();
//       try
//       {
//         var context = services.GetRequiredService<StoreContext>();
//         await context.Database.MigrateAsync();
//         await StoreContextSeed.SeedAsync(context, loggerFactory);
//       }
//       catch (Exception ex)
//       {
//         var logger = loggerFactory.CreateLogger<Program>();
//         logger.LogError(ex, "An error occurred during migration");
//       }
//     }

//     host.Run();
//   }

//   public static IHostBuilder CreateHostBuilder(string[] args) =>
//       Host.CreateDefaultBuilder(args)
//           .ConfigureWebHostDefaults(webBuilder =>
//           {
//             webBuilder.UseStartup<Startup>();
//           });
// }
