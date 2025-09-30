using Domain.Primitives;

namespace Domain.Entities;

public sealed class SaleItem : Entity
{
    internal SaleItem(Guid id, Guid saleId, Guid productId, int quantity, decimal unitPrice) : base(id)
    {
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
    
    private SaleItem() { }
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Total => UnitPrice * Quantity;
}
