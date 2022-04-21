using System.Linq.Expressions;
using Solutions.Dotnet.Core.Entities;

namespace Solutions.Dotnet.Core.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
  public ProductsWithTypesAndBrandsSpecification(
    string sort,
    int? brandId,
    int? typeId) : base(criteria: product =>
    (!brandId.HasValue || product.ProductBrandId == brandId) &&
    (!typeId.HasValue || product.ProductTypeId == typeId)
  )
  {
    AddInclude(p => p.ProductType);
    AddInclude(p => p.ProductBrand);
    AddOrderBy(p => p.Name);

    if (!string.IsNullOrEmpty(sort))
    {
      switch (sort)
      {
        case "priceAsc":
          AddOrderBy(p => p.Price);
          break;
        case "priceDesc":
          AddOrderByDescending(p => p.Price);
          break;
        default:
          AddOrderBy(p => p.Name);
          break;
      }
    }
  }

  public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
  {
    AddInclude(p => p.ProductType);
    AddInclude(p => p.ProductBrand);
  }
}
