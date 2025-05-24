using app_movie_server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace app_movie_server.Data
{
    /// <summary>
    /// Representa el contexto de base de datos principal de la aplicación, gestionando las entidades y su mapeo a la base de datos.
    /// Hereda de <see cref="IdentityDbContext{AppUser}"/> para integrar la gestión de usuarios y roles.
    /// </summary>
    /// <remarks>
    /// Esta clase configura los conjuntos de datos (DbSet) para las entidades principales de la aplicación, incluyendo categorías, películas y usuarios.
    /// Permite la integración con Entity Framework Core y ASP.NET Identity.
    /// </remarks>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Usuarios { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}