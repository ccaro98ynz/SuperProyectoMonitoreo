using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MonitoreoREST.Models;

public partial class BdMonitoreoContext : DbContext
{
    public BdMonitoreoContext()
    {
    }

    public BdMonitoreoContext(DbContextOptions<BdMonitoreoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Componente> Componentes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<OrdenesCompra> OrdenesCompras { get; set; }

    public virtual DbSet<OrdenesCompraDetalle> OrdenesCompraDetalles { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<ProveedoresComponente> ProveedoresComponentes { get; set; }

    public virtual DbSet<RecepcionDetalle> RecepcionDetalles { get; set; }

    public virtual DbSet<Recepcione> Recepciones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:Monitoreo");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.Property(e => e.IdCategoria)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_categoria");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Componente>(entity =>
        {
            entity.HasKey(e => e.IdComponente).HasName("componentes_pkey");

            entity.ToTable("componentes");

            entity.HasIndex(e => e.Codigo, "componentes_codigo_key").IsUnique();

            entity.Property(e => e.IdComponente)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_componente");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .HasColumnName("codigo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .HasColumnName("imagen_url");
            entity.Property(e => e.Marca)
                .HasMaxLength(100)
                .HasColumnName("marca");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.StockMaximo)
                .HasPrecision(14, 3)
                .HasColumnName("stock_maximo");
            entity.Property(e => e.StockMinimo)
                .HasPrecision(14, 3)
                .HasColumnName("stock_minimo");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(20)
                .HasColumnName("unidad_medida");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Componentes)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_componentes_categorias");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.HasIndex(e => e.Email, "employees_email_key").IsUnique();

            entity.Property(e => e.EmployeeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("employee_id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.RolId).HasColumnName("rol_id");

            entity.HasOne(d => d.Rol).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employees_rol");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdInventario).HasName("inventario_pkey");

            entity.ToTable("inventario");

            entity.HasIndex(e => e.IdComponente, "inventario_id_componente_key").IsUnique();

            entity.Property(e => e.IdInventario)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_inventario");
            entity.Property(e => e.CantidadDisponible)
                .HasPrecision(14, 3)
                .HasColumnName("cantidad_disponible");
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.IdComponente).HasColumnName("id_componente");

            entity.HasOne(d => d.IdComponenteNavigation).WithOne(p => p.Inventario)
                .HasForeignKey<Inventario>(d => d.IdComponente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inventario_componente");
        });

        modelBuilder.Entity<OrdenesCompra>(entity =>
        {
            entity.HasKey(e => e.IdOrdenCompra).HasName("ordenes_compra_pkey");

            entity.ToTable("ordenes_compra");

            entity.HasIndex(e => e.FolioOrden, "ordenes_compra_folio_orden_key").IsUnique();

            entity.Property(e => e.IdOrdenCompra)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_orden_compra");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pendiente'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.FechaEsperadaRecibo).HasColumnName("fecha_esperada_recibo");
            entity.Property(e => e.FechaOrden)
                .HasDefaultValueSql("now()")
                .HasColumnName("fecha_orden");
            entity.Property(e => e.FechaRecibo).HasColumnName("fecha_recibo");
            entity.Property(e => e.FolioOrden)
                .HasMaxLength(50)
                .HasColumnName("folio_orden");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.OrdenesCompras)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ordenes_proveedores");
        });

        modelBuilder.Entity<OrdenesCompraDetalle>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("ordenes_compra_detalle_pkey");

            entity.ToTable("ordenes_compra_detalle");

            entity.Property(e => e.IdDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_detalle");
            entity.Property(e => e.CantidadOrdenada).HasColumnName("cantidad_ordenada");
            entity.Property(e => e.IdComponente).HasColumnName("id_componente");
            entity.Property(e => e.IdOrdenCompra).HasColumnName("id_orden_compra");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(12, 2)
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdComponenteNavigation).WithMany(p => p.OrdenesCompraDetalles)
                .HasForeignKey(d => d.IdComponente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_componente");

            entity.HasOne(d => d.IdOrdenCompraNavigation).WithMany(p => p.OrdenesCompraDetalles)
                .HasForeignKey(d => d.IdOrdenCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_orden");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.HasIndex(e => e.Codigo, "proveedores_codigo_key").IsUnique();

            entity.Property(e => e.IdProveedor)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_proveedor");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .HasColumnName("codigo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<ProveedoresComponente>(entity =>
        {
            entity.HasKey(e => new { e.IdProveedor, e.IdComponente }).HasName("pk_proveedores_componentes");

            entity.ToTable("proveedores_componentes");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.IdComponente).HasColumnName("id_componente");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.CodigoProveedor)
                .HasMaxLength(100)
                .HasColumnName("codigo_proveedor");
            entity.Property(e => e.Precio)
                .HasPrecision(12, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.IdComponenteNavigation).WithMany(p => p.ProveedoresComponentes)
                .HasForeignKey(d => d.IdComponente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_componente");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.ProveedoresComponentes)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_proveedor");
        });

        modelBuilder.Entity<RecepcionDetalle>(entity =>
        {
            entity.HasKey(e => e.IdRecepcionDetalle).HasName("recepcion_detalle_pkey");

            entity.ToTable("recepcion_detalle");

            entity.Property(e => e.IdRecepcionDetalle)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_recepcion_detalle");
            entity.Property(e => e.CantidadMerma)
                .HasPrecision(14, 3)
                .HasColumnName("cantidad_merma");
            entity.Property(e => e.CantidadRechazada)
                .HasPrecision(14, 3)
                .HasColumnName("cantidad_rechazada");
            entity.Property(e => e.CantidadRecibida)
                .HasPrecision(14, 3)
                .HasColumnName("cantidad_recibida");
            entity.Property(e => e.EstadoQa)
                .HasMaxLength(30)
                .HasColumnName("estado_qa");
            entity.Property(e => e.IdComponente).HasColumnName("id_componente");
            entity.Property(e => e.IdDetalleOrden).HasColumnName("id_detalle_orden");
            entity.Property(e => e.IdRecepcion).HasColumnName("id_recepcion");
            entity.Property(e => e.NumeroBatch)
                .HasMaxLength(100)
                .HasColumnName("numero_batch");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.IdComponenteNavigation).WithMany(p => p.RecepcionDetalles)
                .HasForeignKey(d => d.IdComponente)
                .HasConstraintName("fk_recepcion_detalle_componente");

            entity.HasOne(d => d.IdDetalleOrdenNavigation).WithMany(p => p.RecepcionDetalles)
                .HasForeignKey(d => d.IdDetalleOrden)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_recepcion_detalle_orden");

            entity.HasOne(d => d.IdRecepcionNavigation).WithMany(p => p.RecepcionDetalles)
                .HasForeignKey(d => d.IdRecepcion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_recepcion_detalle_recepcion");
        });

        modelBuilder.Entity<Recepcione>(entity =>
        {
            entity.HasKey(e => e.IdRecepcion).HasName("recepciones_pkey");

            entity.ToTable("recepciones");

            entity.Property(e => e.IdRecepcion)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_recepcion");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .HasColumnName("estado");
            entity.Property(e => e.FechaRecepcion)
                .HasDefaultValueSql("now()")
                .HasColumnName("fecha_recepcion");
            entity.Property(e => e.IdOrdenCompra).HasColumnName("id_orden_compra");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.IdOrdenCompraNavigation).WithMany(p => p.Recepciones)
                .HasForeignKey(d => d.IdOrdenCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_recepciones_orden");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "roles_nombre_key").IsUnique();

            entity.Property(e => e.RolId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("rol_id");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
