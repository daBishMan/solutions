
using Solutions.Dotnet.Core.Entities;
using Solutions.Dotnet.Core.Specifications;

namespace Solutions.Dotnet.Core.Interfaces;
public interface IGenericRepository<T> where T : BaseEntity
{
  Task<T> GetByIdAsync(int id);
  Task<IReadOnlyList<T>> ListAllAsync();
  Task<T> GetEntityWithSpec(ISpecification<T> spec);
  Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
}
