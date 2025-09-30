using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context) => _context = context;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}
