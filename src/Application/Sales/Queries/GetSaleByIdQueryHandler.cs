using Application.Sales.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
namespace Application.Sales.Queries;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    
    public GetSaleByIdQueryHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }
    public async Task<SaleResponse> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Sale), request.Id);

        return _mapper.Map<SaleResponse>(sale);
    }
}
