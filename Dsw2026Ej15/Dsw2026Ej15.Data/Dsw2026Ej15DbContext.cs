using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data;

public class Dsw2026Ej15DbContext : DbContext
{
    // Constructor que recibe las opciones por inyección de dependencias
    public Dsw2026Ej15DbContext(DbContextOptions<Dsw2026Ej15DbContext> options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specialities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API para configurar límites y restricciones
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LicenseNumber).HasMaxLength(50).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}