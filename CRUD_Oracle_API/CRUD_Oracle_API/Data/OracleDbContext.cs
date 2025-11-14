// Data/OracleDbContext.cs
using Microsoft.EntityFrameworkCore;
using CRUD_Oracle_API.Models;

namespace CRUD_Oracle_API.Data
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
        {
        }

        // === DbSets ===
        public DbSet<Departamento> Departamentos => Set<Departamento>();
        public DbSet<Ciudad> Ciudades => Set<Ciudad>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Accion> Acciones => Set<Accion>();
        public DbSet<Log> Logs => Set<Log>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === DEPARTAMENTO ===
            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.ToTable("DEPARTAMENTO");
                entity.HasKey(e => e.IdDepartamento);
                entity.Property(e => e.IdDepartamento)
                      .HasColumnName("IDDEPARTAMENTO")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.NombreDepartamento)
                      .HasColumnName("NOMBREDEPARTAMENTO")
                      .HasMaxLength(50)
                      .IsRequired();
            });

            // === CIUDAD ===
            modelBuilder.Entity<Ciudad>(entity =>
            {
                entity.ToTable("CIUDAD");
                entity.HasKey(e => e.IdCiudad);
                entity.Property(e => e.IdCiudad)
                      .HasColumnName("IDCIUDAD")
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.NombreCiudad)
                      .HasColumnName("NOMBRECIUDAD")
                      .HasMaxLength(50)
                      .IsRequired();
                entity.Property(e => e.IdDepartamento)
                      .HasColumnName("IDDEPARTAMENTO")
                      .IsRequired();
                entity.HasOne(d => d.Departamento)
                      .WithMany(c => c.Ciudades)
                      .HasForeignKey(e => e.IdDepartamento)
                      .HasConstraintName("FK_DEPARTAMENTO");
            });

            // === USUARIO ===
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIOS");
                entity.HasKey(e => e.Cc);
                entity.Property(e => e.Cc)
                      .HasColumnName("CC")
                      .ValueGeneratedNever();
                entity.Property(e => e.Nombre)
                      .HasColumnName("NOMBRE")
                      .HasMaxLength(50)
                      .IsRequired();
                entity.Property(e => e.FechaNacimiento)
                      .HasColumnName("FECHANACIMIENTO")
                      .HasColumnType("DATE")
                      .IsRequired();
                entity.Property(e => e.IdCiudad)
                      .HasColumnName("IDCIUDAD")
                      .IsRequired();
                entity.HasOne(u => u.Ciudad)
                      .WithMany(c => c.Usuarios)
                      .HasForeignKey(u => u.IdCiudad)
                      .HasConstraintName("IDUSUARIO");

                // CHECK constraint para CC (8 o 10 dígitos)
                entity.HasCheckConstraint("CHECKCC",
                    "LENGTH(TO_CHAR(CC)) IN (8, 10) AND CC > 0");
            });

            // === ACCION ===
            modelBuilder.Entity<Accion>(entity =>
            {
                entity.ToTable("ACCIONES");
                entity.HasKey(e => e.TipoAccion);
                entity.Property(e => e.TipoAccion)
                      .HasColumnName("TIPOACCION")
                      .HasMaxLength(20)
                      .IsRequired();
            });

            // === LOG ===
            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("LOGS");
                entity.HasKey(e => e.IdLog);
                entity.Property(e => e.IdLog)
                      .HasColumnName("IDLOG")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Cc)
                      .HasColumnName("CC")
                      .IsRequired();

                entity.Property(e => e.FechaIngreso)
                      .HasColumnName("FECHAINGRESO")
                      .HasColumnType("DATE")
                      .HasDefaultValueSql("SYSDATE")
                      .IsRequired();

                entity.Property(e => e.TipoAccion)
                      .HasColumnName("TIPOACCION")
                      .HasMaxLength(20)
                      .IsRequired();

                // FK a USUARIOS (unidireccional)
                entity.HasOne(l => l.Usuario)
                      .WithMany()
                      .HasForeignKey(l => l.Cc)
                      .HasConstraintName("CCLOG");

                // FK a ACCIONES (unidireccional)
                entity.HasOne(l => l.Accion)
                      .WithMany()
                      .HasForeignKey(l => l.TipoAccion)
                      .HasConstraintName("ACCIONES");
            });

            // === SOLUCIÓN BOOLEANOS: True/False → 1/0 (Oracle no soporta bool) ===
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetColumnType("NUMBER(1)");
                        property.SetDefaultValue(0);
                        property.SetValueConverter(
                            new Microsoft.EntityFrameworkCore.Storage.ValueConversion
                                .BoolToZeroOneConverter<Int16>());
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}