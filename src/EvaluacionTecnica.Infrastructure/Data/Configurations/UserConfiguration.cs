using EvaluacionTecnica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluacionTecnica.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Apellido)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Cedula)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(u => u.Cedula)
            .IsUnique();

        builder.Property(u => u.Usuario_Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.Usuario_Nombre)
            .IsUnique();

        builder.Property(u => u.Contraseña)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Fecha_Nacimiento)
            .IsRequired();

        builder.Property(u => u.Usuario_Transaccion)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Fecha_Transaccion)
            .IsRequired();

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new User
            {
                Id = 1,
                RoleId = 1,
                Nombre = "Simetrica",
                Apellido = "Consulting",
                Cedula = "25322522135",
                Usuario_Nombre = "ADMIN",
                Contraseña = "ADMIN",
                Fecha_Nacimiento = new DateTime(1990, 1, 1),
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = new DateTime(2024, 1, 1)
            },
            new User
            {
                Id = 2,
                RoleId = 2,
                Nombre = "Nombre_desarrollador",
                Apellido = "Consulting",
                Cedula = "0000000000",
                Usuario_Nombre = "DESARROLLADOR",
                Contraseña = "APLICANTE",
                Fecha_Nacimiento = new DateTime(2000, 2, 25),
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = new DateTime(2024, 1, 1)
            }
        );
    }
}
