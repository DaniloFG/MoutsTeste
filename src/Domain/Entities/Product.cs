using Domain.Primitives;

namespace Domain.Entities;

public sealed class Product : Entity
{
    public Product(Guid id, string name, decimal price, string sku) : base(id)
    {
        Name = name;
        Price = price;
        Sku = sku;
    }

    private Product() { }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Sku { get; private set; }
}
