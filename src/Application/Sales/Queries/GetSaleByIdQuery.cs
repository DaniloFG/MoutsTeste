using Application.Sales.Common;
using MediatR;
namespace Application.Sales.Queries;

public record GetSaleByIdQuery(Guid Id) : IRequest<SaleResponse>;
