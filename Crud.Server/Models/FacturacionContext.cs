using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crud.Server.Models;

public partial class FacturacionContext : DbContext
{
    public FacturacionContext()
    {
    }

    public FacturacionContext(DbContextOptions<FacturacionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetalleOrden> DetalleOrdens { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<ImgsProducto> ImgsProductos { get; set; }

    public virtual DbSet<OrdenVentum> OrdenVenta { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=AKS; Database=facturacion; User Id=sa; Password=sa; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetalleOrden>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PK__DetalleO__6E19D6FAE281D4CF");

            entity.ToTable("DetalleOrden");

            entity.Property(e => e.DetalleId).HasColumnName("DetalleID");
            entity.Property(e => e.OrdenId).HasColumnName("OrdenID");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Orden).WithMany(p => p.DetalleOrdens)
                .HasForeignKey(d => d.OrdenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrdenDetalle");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleOrdens)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductoDetalle");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Error_Log");

            entity.Property(e => e.FechaHora).HasColumnType("datetime");
        });

        modelBuilder.Entity<ImgsProducto>(entity =>
        {
            entity.HasKey(e => e.IdImagen);

            entity.Property(e => e.IdImagen).HasColumnName("idImagen");
            entity.Property(e => e.IdProPer).HasColumnName("idProPer");
            entity.Property(e => e.Imagen)
                .HasColumnType("image")
                .HasColumnName("imagen");

            entity.HasOne(d => d.IdProPerNavigation).WithMany(p => p.ImgsProductos)
                .HasForeignKey(d => d.IdProPer)
                .HasConstraintName("FK_ImgsProductos_Producto");
        });

        modelBuilder.Entity<OrdenVentum>(entity =>
        {
            entity.HasKey(e => e.OrdenId).HasName("PK__OrdenVen__C088A4E45215C2EA");

            entity.Property(e => e.OrdenId).HasColumnName("OrdenID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("('Abierta')");
            entity.Property(e => e.FechaVenta).HasColumnType("datetime");
            entity.Property(e => e.TotalVenta).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.OrdenVenta)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClienteOrden");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE836725DB35");

            entity.ToTable("Producto");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Cliente__71ABD0A7A66333DD");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Apellido).HasMaxLength(50);
            entity.Property(e => e.Cedula).HasMaxLength(10);
            entity.Property(e => e.Direccion).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.Foto).HasColumnType("image");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(10);

            entity.HasOne(d => d.IdRolPerNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRolPer)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
