using Application.Abstractions.Data;
using Application.Sales.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Sales.Commands;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSaleCommandHandler(ISaleRepository saleRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SaleResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = new Sale(Guid.NewGuid(), request.CustomerId);

        foreach (var itemCommand in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemCommand.ProductId, cancellationToken)
                ?? throw new NotFoundException(nameof(Product), itemCommand.ProductId);
                
            sale.AddItem(product, itemCommand.Quantity);
        }

        await _saleRepository.AddAsync(sale, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<SaleResponse>(sale);
    }
}
