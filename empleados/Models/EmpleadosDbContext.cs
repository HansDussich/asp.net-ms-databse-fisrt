using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace empleados.Models;

public partial class EmpleadosDbContext : DbContext
{
    public EmpleadosDbContext()
    {
    }

    public EmpleadosDbContext(DbContextOptions<EmpleadosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Empleados> Empleados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleados>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Empleado__3214EC071B1C64C4");

            entity.HasIndex(e => e.Correo, "UQ__Empleado__60695A19D4EC5C4B").IsUnique();

            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
