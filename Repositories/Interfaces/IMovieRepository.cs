using app_movie_server.Models;

namespace app_movie_server.Repositories.Interfaces
{
    /// <summary>
    /// Define las operaciones de acceso a datos para la entidad <see cref="Movie"/>.
    /// Proporciona métodos para obtener, buscar, crear, actualizar y eliminar películas,
    /// así como para verificar su existencia y guardar los cambios en el repositorio.
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Obtiene una colección paginada de películas.
        /// </summary>
        /// <param name="pageNumber">Número de página a recuperar.</param>
        /// <param name="pageSize">Cantidad de elementos por página.</param>
        /// <returns>
        /// Una colección de objetos <see cref="Movie"/> correspondiente a la página solicitada.
        /// </returns>
        ICollection<Movie> GetMovies(int pageNumber, int pageSize);

        /// <summary>
        /// Obtiene el número total de películas registradas en el repositorio.
        /// </summary>
        /// <returns>
        /// Un entero que representa la cantidad total de películas.
        /// </returns>
        int GetTotalMovies();

        /// <summary>
        /// Obtiene una colección de películas asociadas a una categoría específica.
        /// </summary>
        /// <param name="categoryId">Identificador único de la categoría.</param>
        /// <returns>
        /// Una colección de objetos <see cref="Movie"/> pertenecientes a la categoría indicada.
        /// </returns>
        ICollection<Movie> GetMoviesByCategory(int categoryId);

        /// <summary>
        /// Busca películas cuyo nombre coincida parcial o totalmente con el valor proporcionado.
        /// </summary>
        /// <param name="name">Nombre o parte del nombre de la película a buscar.</param>
        /// <returns>
        /// Una secuencia de objetos <see cref="Movie"/> que cumplen con el criterio de búsqueda.
        /// </returns>
        IEnumerable<Movie> SearchMovie(string name);

        /// <summary>
        /// Obtiene una película específica según su identificador único.
        /// </summary>
        /// <param name="movieId">Identificador único de la película.</param>
        /// <returns>
        /// El objeto <see cref="Movie"/> correspondiente al identificador proporcionado, o <c>null</c> si no existe.
        /// </returns>
        Movie GetMovie(int movieId);

        /// <summary>
        /// Verifica si existe una película con el identificador especificado.
        /// </summary>
        /// <param name="movieId">Identificador único de la película.</param>
        /// <returns>
        /// <c>true</c> si la película existe; en caso contrario, <c>false</c>.
        /// </returns>
        bool MovieExists(int movieId);

        /// <summary>
        /// Verifica si existe una película con el nombre especificado.
        /// </summary>
        /// <param name="name">Nombre de la película.</param>
        /// <returns>
        /// <c>true</c> si la película existe; en caso contrario, <c>false</c>.
        /// </returns>
        bool MovieExists(string name);

        /// <summary>
        /// Crea una nueva película en el repositorio.
        /// </summary>
        /// <param name="movie">Objeto <see cref="Movie"/> que representa la película a crear.</param>
        /// <returns>
        /// <c>true</c> si la operación fue exitosa; en caso contrario, <c>false</c>.
        /// </returns>
        bool CreateMovie(Movie movie);

        /// <summary>
        /// Actualiza la información de una película existente en el repositorio.
        /// </summary>
        /// <param name="movie">Objeto <see cref="Movie"/> con los datos actualizados.</param>
        /// <returns>
        /// <c>true</c> si la operación fue exitosa; en caso contrario, <c>false</c>.
        /// </returns>
        bool UpdateMovie(Movie movie);

        /// <summary>
        /// Elimina una película existente del repositorio.
        /// </summary>
        /// <param name="movie">Objeto <see cref="Movie"/> que representa la película a eliminar.</param>
        /// <returns>
        /// <c>true</c> si la operación fue exitosa; en caso contrario, <c>false</c>.
        /// </returns>
        bool DeleteMovie(Movie movie);

        /// <summary>
        /// Guarda los cambios realizados en el repositorio.
        /// </summary>
        /// <returns>
        /// <c>true</c> si los cambios se guardaron correctamente; en caso contrario, <c>false</c>.
        /// </returns>
        bool Save();
    }
}