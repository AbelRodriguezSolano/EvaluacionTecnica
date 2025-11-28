using EvaluacionTecnica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluacionTecnica.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(r => r.Nombre)
            .IsUnique();

        builder.Property(r => r.Usuario_Transaccion)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Fecha_Transaccion)
            .IsRequired();

        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Role
            {
                Id = 1,
                Nombre = "ADMIN",
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = new DateTime(2024, 1, 1)
            },
            new Role
            {
                Id = 2,
                Nombre = "DESARROLLADOR",
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = new DateTime(2024, 1, 1)
            }
        );
    }
}
