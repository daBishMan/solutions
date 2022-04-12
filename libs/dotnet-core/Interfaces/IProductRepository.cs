using Dotnet.Core.Entities;

namespace Dotnet.Core.Interfaces
{
  public interface IProductRepository
  {
    Task<Product> GetProductByIdAsync(int id);

    Task<IReadOnlyList<Product>> GetProductsAsync();
  }
}
