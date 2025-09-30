using Application.Sales.Common;
using MediatR;
namespace Application.Sales.Commands;

public record CreateSaleItemCommand(Guid ProductId, int Quantity);
public record CreateSaleCommand(Guid CustomerId, List<CreateSaleItemCommand> Items) : IRequest<SaleResponse>;
