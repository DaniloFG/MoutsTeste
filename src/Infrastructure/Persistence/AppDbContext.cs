using Application.Abstractions.Data;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Sale> Sales { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.GetDomainEvents().Any())
            .SelectMany(e => e.GetDomainEvents())
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var aggregateRoot in ChangeTracker.Entries<AggregateRoot>())
        {
            aggregateRoot.Entity.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
