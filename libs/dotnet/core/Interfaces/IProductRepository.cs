using Solutions.Dotnet.Core.Entities;

namespace Solutions.Dotnet.Core.Interfaces;

public interface IProductRepository
{
  Task<Product> GetProductByIdAsync(int id);

  Task<IReadOnlyList<Product>> GetProductsAsync();
}
  