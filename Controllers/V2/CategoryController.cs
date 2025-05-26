using app_movie_server.Repositories.Interfaces;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace app_movie_server.Controllers.V2
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con las categorías en el sistema - Versión 2.0.
    /// </summary>
    /// <remarks>
    /// Esta es la versión 2.0 del controlador de categorías que proporciona funcionalidades actualizadas
    /// y mejoradas para la gestión de categorías de películas. Reemplaza las funcionalidades obsoletas
    /// de la versión 1.0.
    /// </remarks>
    /// <param name="categoryRepository">Repositorio de categorías para acceder a la base de datos.</param>
    /// <param name="mapper">Instancia de AutoMapper para realizar conversiones entre entidades y DTOs.</param>
    /// <returns>
    /// Una instancia de <see cref="CategoryController"/> versión 2.0 que permite gestionar las operaciones relacionadas con las categorías.
    /// </returns>
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository categoryRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtiene una lista de cadenas de ejemplo mejorada para la versión 2.0.
        /// </summary>
        /// <returns>Una colección de cadenas de ejemplo en inglés.</returns>
        /// <response code="200">Lista de cadenas obtenida exitosamente.</response>
        /// <remarks>
        /// Este es el endpoint mejorado de la versión 2.0 que reemplaza el endpoint obsoleto de la versión 1.0.
        /// Proporciona valores en inglés y una implementación optimizada.
        /// </remarks>
        [HttpGet("GetString")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<string> Get()
        {
            return ["value1", "value2"];
        }
    }
}