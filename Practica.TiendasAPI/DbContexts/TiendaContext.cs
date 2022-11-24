using Practica.TiendasAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace Practica.TiendasAPI.DbContexts
{
    public class TiendaContext : IdentityDbContext
    {
        public DbSet<Tienda> Tiendas { get; set; } = null!;
        public DbSet<Producto> Productos { get; set; } = null!;
        
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName="Usuario",
                    NormalizedUserName="USUARIO",
                    Email = "correo_prueba@hotmail.com",
                    NormalizedEmail = "CORREO_PRUEBA@HOTMAIL.COM",
                    PasswordHash = hasher.HashPassword(null,"password1234")
                });
            modelBuilder.Entity<Tienda>().HasData(
                new Tienda(){ Id = 1, Name = "Panadería Paqui", Description = "Pan recién hecho." },
                new Tienda() { Id = 2, Name = "Bazar Todo a 1", Description = "Todo a 1€." },
                new Tienda() { Id = 3, Name = "Supermercados MAS" },
                new Tienda() { Id = 4, Name = "Verdulería La Fresca" });
            modelBuilder.Entity<Producto>().HasData(
                new Producto() { Id = 1, Name = "Pan integral", ShopId = 1, Price = 0.3 },
                new Producto() { Id = 2, Name = "Bizcocho", Description = "Porción de bizcocho casero.", ShopId = 1, Price = 0.3 },
                new Producto() { Id = 3, Name = "Cruzcampo 1L", Description = "Botella de 1 litro de cerveza.", ShopId = 2, Price = 1.0 },
                new Producto() { Id = 4, Name = "Agua 2L", Description = "Botella de 1 litro de agua.", ShopId = 2, Price = 1.0 },
                new Producto() { Id = 5, Name = "Agua 2L", Description = "Botella de 1 litro de agua.", ShopId = 3, Price = 1.2 },
                new Producto() { Id = 6, Name = "Tomate", Description = "1 Kilogramo de tomates.", ShopId = 4, Price = 1.99 },
                new Producto() { Id = 7, Name = "Calabacín", Description = "1 Kilogramo de calabacín.", ShopId = 4, Price = 1.99 });
        }
    }

}
