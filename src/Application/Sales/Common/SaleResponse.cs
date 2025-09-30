namespace Application.Sales.Common;

public record SaleItemResponse(Guid ProductId, int Quantity, decimal UnitPrice, decimal Total);

public record SaleResponse(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    decimal DiscountApplied,
    List<SaleItemResponse> Items);
