using Microsoft.EntityFrameworkCore;
using Solutions.Dotnet.Core.Entities;
using Solutions.Dotnet.Core.Specifications;

namespace Solutions.Dotnet.Infrastructure.Data;

public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
  public static IQueryable<TEntity> GetQuery(
    IQueryable<TEntity> inputQuery,
    ISpecification<TEntity> specification)
  {
    var query = inputQuery;

    // modify the IQueryable using the specification's criteria expression
    if (specification.Criteria != null)
    {
      query = query.Where(specification.Criteria);
    }

    if (specification.OrderBy != null)
    {
      query = query.OrderBy(specification.OrderBy);
    }

    if (specification.OrderByDescending != null)
    {
      query = query.OrderByDescending(specification.OrderByDescending);
    }

    // Ensure we page after ordering and filtering
    if(specification.IsPagingEnabled)
    {
      query = query.Skip(specification.Skip).Take(specification.Take);
    }

    // Includes all expression-based includes
    query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

    return query;
  }
}
