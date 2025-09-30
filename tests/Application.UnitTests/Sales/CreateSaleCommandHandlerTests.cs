using Application.Abstractions.Data;
using Application.Common.Mappings;
using Application.Sales.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Sales;

public class CreateSaleCommandHandlerTests
{
private readonly Mock<ISaleRepository> _saleRepositoryMock;
private readonly Mock<IProductRepository> _productRepositoryMock;
private readonly Mock<IUnitOfWork> _unitOfWorkMock;
private readonly IMapper _mapper;

public CreateSaleCommandHandlerTests()
{
    _saleRepositoryMock = new Mock<ISaleRepository>();
    _productRepositoryMock = new Mock<IProductRepository>();
    _unitOfWorkMock = new Mock<IUnitOfWork>();
    
    var mappingConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MappingProfile());
    });
    _mapper = mappingConfig.CreateMapper();
}

[Fact]
public async Task Handle_Should_ReturnSaleResponse_WhenCommandIsValid()
{
    // Arrange
    var productId = Guid.NewGuid();
    var command = new CreateSaleCommand(Guid.NewGuid(), new List<CreateSaleItemCommand>
    {
        new(productId, 2)
    });

    var product = new Product(productId, "Test Product", 100m, "TP01");

    _productRepositoryMock.Setup(
        x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
        .ReturnsAsync(product);
        
    _unitOfWorkMock.Setup(
        x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
        .ReturnsAsync(1);

    var handler = new CreateSaleCommandHandler(
        _saleRepositoryMock.Object,
        _productRepositoryMock.Object,
        _unitOfWorkMock.Object,
        _mapper);

    // Act
    var result = await handler.Handle(command, default);

    // Assert
    result.Should().NotBeNull();
    result.CustomerId.Should().Be(command.CustomerId);
    result.Items.Should().HaveCount(1);
    
    _saleRepositoryMock.Verify(x => x.AddAsync(It.Is<Sale>(s => s.Items.Count == 1), It.IsAny<CancellationToken>()), Times.Once);
    _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
}

[Fact]
public async Task Handle_Should_ThrowNotFoundException_WhenProductDoesNotExist()
{
    // Arrange
    var productId = Guid.NewGuid();
    var command = new CreateSaleCommand(Guid.NewGuid(), new List<CreateSaleItemCommand>
    {
        new(productId, 2)
    });

    _productRepositoryMock.Setup(
        x => x.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Product?)null);

    var handler = new CreateSaleCommandHandler(
        _saleRepositoryMock.Object,
        _productRepositoryMock.Object,
        _unitOfWorkMock.Object,
        _mapper);

    // Act
    Func<Task> act = async () => await handler.Handle(command, default);

    // Assert
    await act.Should().ThrowAsync<Domain.Exceptions.NotFoundException>();
}
}
