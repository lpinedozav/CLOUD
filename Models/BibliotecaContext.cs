using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace CLOUD.Models;

public partial class BibliotecaContext : DbContext
{
    public BibliotecaContext()
    {
    }

    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=ec2-54-145-10-149.compute-1.amazonaws.com;port=3306;database=biblioteca;user=root;password=root123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.8.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.IdLibro).HasName("PRIMARY");

            entity.ToTable("libros");

            entity.Property(e => e.IdLibro)
                .HasMaxLength(20)
                .HasColumnName("id_libro");
            entity.Property(e => e.Autor)
                .HasMaxLength(100)
                .HasColumnName("autor");
            entity.Property(e => e.Copia)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("copia");
            entity.Property(e => e.Disponible)
                .HasDefaultValueSql("'1'")
                .HasColumnName("disponible");
            entity.Property(e => e.NombreLibro)
                .HasMaxLength(100)
                .HasColumnName("nombre_libro");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.IdPrestamo).HasName("PRIMARY");

            entity.ToTable("prestamos");

            entity.HasIndex(e => e.IdLibro, "id_libro");

            entity.HasIndex(e => e.RunUsuario, "run_usuario");

            entity.Property(e => e.IdPrestamo)
                .HasColumnType("int(11)")
                .HasColumnName("id_prestamo");
            entity.Property(e => e.Devuelto)
                .HasDefaultValueSql("'1'")
                .HasColumnName("devuelto");
            entity.Property(e => e.FechaDevolucion).HasColumnName("fecha_devolucion");
            entity.Property(e => e.FechaPrestamo).HasColumnName("fecha_prestamo");
            entity.Property(e => e.IdLibro)
                .HasMaxLength(20)
                .HasColumnName("id_libro");
            entity.Property(e => e.RunUsuario)
                .HasMaxLength(12)
                .HasColumnName("run_usuario");

            entity.HasOne(d => d.IdLibroNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.IdLibro)
                .HasConstraintName("prestamos_ibfk_1");

            entity.HasOne(d => d.RunUsuarioNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.RunUsuario)
                .HasConstraintName("prestamos_ibfk_2");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Run).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.Property(e => e.Run)
                .HasMaxLength(12)
                .HasColumnName("run");
            entity.Property(e => e.Direccion)
                .HasMaxLength(150)
                .HasColumnName("direccion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
