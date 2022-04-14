using System.Text.Json;
using Dotnet.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Dotnet.Infrastructure.Data;
public class StoreContextSeed
{
  public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
  {
    try
    {
      // ! Not sure why we need to 3 three slashes for the first path but not the rest???
      if (!context.ProductBrands.Any())
      {
        var brandsData =
          File.ReadAllText("../../../Libs/dotnet-infrastructure/Data/SeedData/brands.json");

        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
        foreach (var brand in brands)
        {
          context.ProductBrands.Add(brand);
        }
        await context.SaveChangesAsync();
      }

      if (!context.ProductTypes.Any())
      {
        var typesData =
          File.ReadAllText("../../Libs/dotnet-infrastructure/Data/SeedData/types.json");
        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
        foreach (var type in types)
        {
          context.ProductTypes.Add(type);
        }
        await context.SaveChangesAsync();
      }

      if (!context.Products.Any())
      {
        var productsData =
          File.ReadAllText("../../Libs/dotnet-infrastructure/Data/SeedData/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(productsData);
        foreach (var product in products)
        {
          context.Products.Add(product);
        }
        await context.SaveChangesAsync();
      }

    }
    catch (Exception ex)
    {
      var logger = loggerFactory.CreateLogger<StoreContextSeed>();
      logger.LogError(ex, "An error occurred during seeding the database.");
    }
  }
}

