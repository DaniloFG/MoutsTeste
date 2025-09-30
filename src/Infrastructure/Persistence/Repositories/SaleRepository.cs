using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence.Repositories;

public sealed class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;
    public SaleRepository(AppDbContext context) => _context = context;
    
    public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default) =>
        await _context.Sales.AddAsync(sale, cancellationToken);

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public void Update(Sale sale) => _context.Sales.Update(sale);
}
