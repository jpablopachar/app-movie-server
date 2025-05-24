using app_movie_server.Data;
using app_movie_server.Models;
using app_movie_server.Repositories.Interfaces;

namespace app_movie_server.Repositories
{
    /// <summary>
    /// Repositorio para la gestión de entidades <see cref="Category"/> en la base de datos.
    /// Proporciona métodos para crear, actualizar, eliminar, consultar y verificar la existencia de categorías.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz <see cref="ICategoryRepository"/> y utiliza <see cref="AppDbContext"/>
    /// para interactuar con la base de datos.
    /// </remarks>
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        private readonly AppDbContext _context = context;

        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public bool CategoryExists(string name)
        {
            return _context.Categories.Any(c => c.Name == name);
        }

        public bool CreateCategory(Category category)
        {
            category.CreatedAt = DateTime.Now;

            _context.Categories.Add(category);

            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);

            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return [.. _context.Categories.OrderBy(c => c.Name)];
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId)!;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool UpdateCategory(Category category)
        {
            category.CreatedAt = DateTime.Now;

            var existingCategory = _context.Categories.Find(category.Id);

            if (existingCategory != null)
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(category);
            }
            else
            {
                _context.Categories.Update(category);
            }

            return Save();
        }
    }
}