using Bogus;
using Domain.Entities;
namespace Infrastructure.Persistence;

public static class DataGenerator
{
    public static void Seed(AppDbContext context)
    {
        if (context.Products.Any()) return;

        var products = new Faker<Product>("pt_BR")
            .CustomInstantiator(f =>
                new Product(Guid.NewGuid(), f.Commerce.ProductName(), decimal.Parse(f.Commerce.Price(10, 1000)), f.Commerce.Ean13()))
            .Generate(20);

        context.Products.AddRange(products);

        context.SaveChanges();
    }
}
