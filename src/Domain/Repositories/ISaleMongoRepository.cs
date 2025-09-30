using Domain.Entities;
namespace Domain.Repositories;

public interface ISaleMongoRepository
{
    Task CreateAsync(Sale sale);
}
