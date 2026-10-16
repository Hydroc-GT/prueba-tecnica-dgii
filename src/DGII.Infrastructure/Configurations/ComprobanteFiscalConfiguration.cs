using DGII.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGII.Infrastructure.Configurations;

public class ComprobanteFiscalConfiguration : IEntityTypeConfiguration<ComprobanteFiscal>
{
    public void Configure(EntityTypeBuilder<ComprobanteFiscal> builder)
    {
        builder.ToTable("ComprobantesFiscales");

        builder.HasKey(c => new { c.RncCedula, c.Ncf });

        builder.Property(c => c.RncCedula)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(c => c.Ncf)
            .HasMaxLength(13)
            .IsRequired();

        builder.Property(c => c.Monto)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.Itbis18)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(c => c.Contribuyente)
            .WithMany(t => t.ComprobantesFiscales)
            .HasForeignKey(c => c.RncCedula)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
