using Domain.Events;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
namespace Application.Sales.Events;

public sealed class SaleCreatedDomainEventHandler : INotificationHandler<SaleCreatedEvent>
{
    private readonly ISaleRepository _saleSqlRepository;
    private readonly ISaleMongoRepository _saleMongoRepository;

    public SaleCreatedDomainEventHandler(ISaleRepository saleSqlRepository, ISaleMongoRepository saleMongoRepository)
    {
        _saleSqlRepository = saleSqlRepository;
        _saleMongoRepository = saleMongoRepository;
    }

    public async Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        var sale = await _saleSqlRepository.GetByIdAsync(notification.SaleId, cancellationToken)
            ?? throw new NotFoundException("Sale", notification.SaleId);

        await _saleMongoRepository.CreateAsync(sale);
    }
}
