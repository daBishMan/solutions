using Microsoft.EntityFrameworkCore;
using Solutions.Dotnet.Core.Entities;
using Solutions.Dotnet.Core.Interfaces;

namespace Solutions.Dotnet.Infrastructure.Data;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
  private readonly StoreContext _context;

  public GenericRepository(StoreContext context)
  {
    _context = context;
  }

  public async Task<T> GetByIdAsync(int id)
  {
    return await _context.Set<T>().FindAsync(id);
  }

  public async Task<IReadOnlyList<T>> ListAllAsync()
  {
    return await _context.Set<T>().ToListAsync();
  }
}
