using System.Linq.Expressions;
using Solutions.Dotnet.Core.Entities;

namespace Solutions.Dotnet.Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
  public ProductsWithTypesAndBrandsSpecification()
  {
    AddInclude(p => p.ProductType);
    AddInclude(p => p.ProductBrand);
  }

  public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
  {
    AddInclude(p => p.ProductType);
    AddInclude(p => p.ProductBrand);
  }
}
