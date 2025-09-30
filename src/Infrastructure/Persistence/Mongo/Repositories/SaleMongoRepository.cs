using Domain.Entities;
using Domain.Repositories;
namespace Infrastructure.Persistence.Mongo.Repositories;

public class SaleMongoRepository : ISaleMongoRepository
{
    private readonly MongoDbContext _context;
    public SaleMongoRepository(MongoDbContext context) => _context = context;
    public async Task CreateAsync(Sale sale) => await _context.Sales.InsertOneAsync(sale);
}
