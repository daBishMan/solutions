using Microsoft.EntityFrameworkCore;
using Solutions.Dotnet.Core.Entities;
using Solutions.Dotnet.Core.Specifications;

namespace Solutions.Dotnet.Infrastructure.Data;

public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
  public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
  {
    var query = inputQuery;

    // modify the IQueryable using the specification's criteria expression
    if (specification.Criteria != null)
    {
      query = query.Where(specification.Criteria);
    }

    // Includes all expression-based includes
    query = specification.Includes.Aggregate(
      query, (current, include) => current.Include(include));

    return query;
  }
}
