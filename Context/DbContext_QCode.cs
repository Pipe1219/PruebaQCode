using API_QCode.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API_QCode.Context
{
    public class DbContext_QCode : DbContext 
    {
        public DbContext_QCode(DbContextOptions<DbContext_QCode> options) : base(options)
        {}

        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<LlaveContrasena> Llave_Contrasenas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //DEFAULTS DE VEHICULOS
            modelBuilder.Entity<Vehiculo>()
                .Property(p => p.VehiculoId)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Vehiculo>()
                .Property(p => p.Valor)
                .HasColumnType("decimal(18,2)");

            //DEFAULTS DE EMPLEADO
            modelBuilder.Entity<Empleado>()
                .Property(p => p.EmpleadoId)
                .HasDefaultValueSql("NEWID()");

            //DEFAULTS DE LLAVECONTRASENA
            modelBuilder.Entity<LlaveContrasena>()
                .Property(p => p.LlaveContrasenaId)
                .HasDefaultValueSql("NEWID()");
        }
    }
}