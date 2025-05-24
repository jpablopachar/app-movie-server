using app_movie_server.Models;

namespace app_movie_server.Repositories.Interfaces
{
    /// <summary>
    /// Define la interfaz para el repositorio de categorías, proporcionando métodos para gestionar entidades de tipo <see cref="Category"/>.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Obtiene una colección de todas las categorías disponibles.
        /// </summary>
        /// <returns>
        /// Una colección de objetos <see cref="Category"/> que representa todas las categorías registradas.
        /// </returns>
        ICollection<Category> GetCategories();

        /// <summary>
        /// Obtiene una categoría específica según su identificador único.
        /// </summary>
        /// <param name="categoryId">Identificador único de la categoría a recuperar.</param>
        /// <returns>
        /// Un objeto <see cref="Category"/> correspondiente al identificador proporcionado, o <c>null</c> si no se encuentra.
        /// </returns>
        Category GetCategory(int categoryId);

        /// <summary>
        /// Verifica si existe una categoría con el identificador especificado.
        /// </summary>
        /// <param name="categoryId">Identificador único de la categoría.</param>
        /// <returns>
        /// <c>true</c> si la categoría existe; en caso contrario, <c>false</c>.
        /// </returns>
        bool CategoryExists(int categoryId);

        /// <summary>
        /// Verifica si existe una categoría con el nombre especificado.
        /// </summary>
        /// <param name="name">Nombre de la categoría.</param>
        /// <returns>
        /// <c>true</c> si la categoría existe; en caso contrario, <c>false</c>.
        /// </returns>
        bool CategoryExists(string name);

        /// <summary>
        /// Crea una nueva categoría en el repositorio.
        /// </summary>
        /// <param name="category">Objeto <see cref="Category"/> que representa la nueva categoría a agregar.</param>
        /// <returns>
        /// <c>true</c> si la operación se realizó correctamente; en caso contrario, <c>false</c>.
        /// </returns>
        bool CreateCategory(Category category);

        /// <summary>
        /// Actualiza la información de una categoría existente.
        /// </summary>
        /// <param name="category">Objeto <see cref="Category"/> con los datos actualizados.</param>
        /// <returns>
        /// <c>true</c> si la actualización fue exitosa; en caso contrario, <c>false</c>.
        /// </returns>
        bool UpdateCategory(Category category);

        /// <summary>
        /// Elimina una categoría del repositorio.
        /// </summary>
        /// <param name="category">Objeto <see cref="Category"/> que representa la categoría a eliminar.</param>
        /// <returns>
        /// <c>true</c> si la eliminación fue exitosa; en caso contrario, <c>false</c>.
        /// </returns>
        bool DeleteCategory(Category category);

        /// <summary>
        /// Guarda los cambios realizados en el repositorio de categorías.
        /// </summary>
        /// <returns>
        /// <c>true</c> si los cambios se guardaron correctamente; en caso contrario, <c>false</c>.
        /// </returns>
        bool Save();
    }
}