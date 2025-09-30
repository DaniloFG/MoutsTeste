using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(s => s.Id);
        builder.Ignore(s => s.TotalAmount);
        builder.Property(s => s.DiscountApplied).HasColumnType("decimal(18,2)");
        builder.HasMany(s => s.Items)
        .WithOne()
        .HasForeignKey(si => si.SaleId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
