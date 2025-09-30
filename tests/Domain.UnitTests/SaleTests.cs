using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Domain.UnitTests;

public class SaleTests
{
    [Fact]
    public void AddItem_Should_CalculateTotalWith10PercentDiscount_WhenTotalItemsAreBetween4And9()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var product1 = new Product(Guid.NewGuid(), "Product A", 10.00m, "SKU001");
        var product2 = new Product(Guid.NewGuid(), "Product B", 20.00m, "SKU002");

        // Act
        sale.AddItem(product1, 3); 
        sale.AddItem(product2, 2);                                    

        // Assert
        var expectedTotal = 70.00m * 0.90m;
        sale.TotalAmount.Should().Be(expectedTotal);
        sale.DiscountApplied.Should().Be(70.00m * 0.10m);
    }

    [Fact]
    public void AddItem_Should_CalculateTotalWith20PercentDiscount_WhenTotalItemsAre10OrMore()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var product = new Product(Guid.NewGuid(), "Product A", 10.00m, "SKU001");

        // Act
        sale.AddItem(product, 10);

        // Assert
        var expectedTotal = 100.00m * 0.80m;
        sale.TotalAmount.Should().Be(expectedTotal);
        sale.DiscountApplied.Should().Be(20.00m);
    }

    [Fact]
    public void AddItem_Should_CalculateTotalWithNoDiscount_WhenTotalItemsAreLessThan4()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var product = new Product(Guid.NewGuid(), "Product A", 10.00m, "SKU001");

        // Act
        sale.AddItem(product, 3);

        // Assert
        sale.TotalAmount.Should().Be(30.00m);
        sale.DiscountApplied.Should().Be(0);
    }

    [Fact]
    public void AddItem_Should_ThrowMaxItemQuantityExceededException_WhenAddingMoreThan20ItemsOfAProduct()
    {
        // Arrange
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());
        var product = new Product(Guid.NewGuid(), "Product A", 10.00m, "SKU001");

        // Act
        Action act = () => sale.AddItem(product, 21);

        // Assert
        act.Should().Throw<MaxItemQuantityExceededException>();
    }

    [Fact]
    public void AddItem_Should_RaiseSaleCreatedDomainEvent_WhenSaleIsCreated()
    {
        // Arrange & Act
        var sale = new Sale(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        sale.GetDomainEvents().Should().HaveCount(1);
        sale.GetDomainEvents().First().Should().BeOfType<Domain.Events.SaleCreatedEvent>();
    }
}
