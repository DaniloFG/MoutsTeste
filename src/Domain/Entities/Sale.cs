using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.Primitives;

namespace Domain.Entities;

public sealed class Sale : AggregateRoot
{
    private readonly List<SaleItem> _items = new();

    public Sale(Guid id, Guid customerId) : base(id)
    {
        CustomerId = customerId;
        Status = SaleStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new SaleCreatedEvent(Guid.NewGuid(), Id, DateTime.UtcNow));
    }

    private Sale() { }

    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public SaleStatus Status { get; private set; }
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
    public decimal TotalAmount => CalculateTotal();
    public decimal DiscountApplied { get; private set; }

    public void AddItem(Product product, int quantity)
    {
        if (Status != SaleStatus.Pending) throw new SaleIsNotPendingException("Cannot add items to a sale that is not in pending status.");

        if (quantity <= 0) throw new InvalidQuantityException("Quantity must be greater than zero.");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);

        if (existingItem is not null) throw new BusinessRuleValidationException("Product already exists in the sale. Consider updating the item instead of adding.");

        if (quantity > 20) throw new MaxItemQuantityExceededException($"Cannot add more than 20 items for product {product.Sku}.");

        var saleItem = new SaleItem(Guid.NewGuid(), Id, product.Id, quantity, product.Price);
        _items.Add(saleItem);
    }

    public void Cancel()
    {
        if (Status == SaleStatus.Completed) throw new BusinessRuleValidationException("Cannot cancel a completed sale.");
        Status = SaleStatus.Cancelled;
    }

    public void Complete()
    {
        if (Status != SaleStatus.Pending) throw new BusinessRuleValidationException("Only pending sales can be completed.");
        Status = SaleStatus.Completed;
    }

    private decimal CalculateTotal()
    {
        var subtotal = _items.Sum(item => item.Total);

        var totalItems = _items.Sum(item => item.Quantity);

        DiscountApplied = 0;

        if (totalItems < 4) return subtotal;

        if (totalItems >= 10 && totalItems <= 20)
        {
            DiscountApplied = subtotal * 0.20m;
            return subtotal - DiscountApplied;
        }

        DiscountApplied = subtotal * 0.10m;

        return subtotal - DiscountApplied;
    }
}
