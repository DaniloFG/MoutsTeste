namespace Domain.Events;

public sealed record SaleCreatedEvent(Guid Id, Guid SaleId, DateTime OccurredOn) : IDomainEvent;
