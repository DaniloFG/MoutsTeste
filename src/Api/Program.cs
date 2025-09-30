using Application.Abstractions.Data;
using Application.Common.Mappings;
using Application.Sales.Commands;
using Application.Sales.Queries;
using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Mongo;
using Infrastructure.Persistence.Mongo.Repositories;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<ISaleMongoRepository, SaleMongoRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateSaleCommand).Assembly));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>();
    DataGenerator.Seed(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/sales", async (CreateSaleCommand command, ISender sender) =>
{
    var result = await sender.Send(command);
    return Results.Created($"/sales/{result.Id}", result);
});

app.MapGet("/sales/{id:guid}", async (Guid id, ISender sender) =>
{
    try
    {
        var query = new GetSaleByIdQuery(id);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }
    catch (Domain.Exceptions.NotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.Run();
