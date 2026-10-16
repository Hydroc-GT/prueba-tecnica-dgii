using DGII.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DGII.Infrastructure.Configurations;

public class ContribuyenteConfiguration : IEntityTypeConfiguration<Contribuyente>
{
    public void Configure(EntityTypeBuilder<Contribuyente> builder)
    {
        builder.ToTable("Contribuyentes");

        builder.HasKey(c => c.RncCedula);

        builder.Property(c => c.RncCedula)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(c => c.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(c => c.Tipo)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Estatus)
            .HasMaxLength(20)
            .IsRequired();
    }
}
