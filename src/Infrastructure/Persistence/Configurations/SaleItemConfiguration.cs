using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Persistence.Configurations;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");
        builder.HasKey(si => si.Id);
        builder.Property(si => si.UnitPrice).HasColumnType("decimal(18,2)");
        builder.Ignore(si => si.Total);
    }
}
