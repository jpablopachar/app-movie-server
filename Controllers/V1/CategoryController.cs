using app_movie_server.Models;
using app_movie_server.Models.Dtos;
using app_movie_server.Repositories.Interfaces;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_movie_server.Controllers.V1
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con las categorías en el sistema.
    /// </summary>
    /// <remarks>
    /// Este controlador permite realizar operaciones CRUD sobre las categorías de películas,
    /// incluyendo la creación, consulta, actualización y eliminación de categorías.
    /// </remarks>
    /// <param name="categoryRepository">Repositorio de categorías para acceder a la base de datos.</param>
    /// <param name="mapper">Instancia de AutoMapper para realizar conversiones entre entidades y DTOs.</param>
    /// <returns>
    /// Una instancia de <see cref="CategoryController"/> que permite gestionar las operaciones relacionadas con las categorías.
    /// </returns>
    [Route("api/v{version:apiVersion}/category")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CategoryController(ICategoryRepository categoryRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtiene una lista de cadenas de ejemplo para pruebas.
        /// </summary>
        /// <returns>Una colección de cadenas de ejemplo.</returns>
        /// <remarks>
        /// Este endpoint está marcado como obsoleto y será removido en futuras versiones.
        /// Se recomienda utilizar el endpoint de la versión 2.0.
        /// </remarks>
        [HttpGet("GetString")]
        [Obsolete("Este endpoint está obsoleto. Por favor utilice el endpoint de la versión 2.0.")]
        public IEnumerable<string> Get()
        {
            return ["valor1", "valor2", "valor3"];
        }

        /// <summary>
        /// Obtiene una lista de todas las categorías disponibles en el sistema.
        /// </summary>
        /// <returns>Una respuesta <see cref="IActionResult"/> que contiene la lista de categorías.</returns>
        /// <response code="200">Lista de categorías obtenida exitosamente.</response>
        /// <response code="403">Acceso denegado. Se requiere autorización.</response>
        /// <remarks>
        /// Este método permite acceso anónimo y utiliza caché para mejorar el rendimiento.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30Seconds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCategoriesV2()
        {
            var categories = _categoryRepository.GetCategories();
            var categoriesDto = new List<CategoryDto>();

            foreach (var category in categories)
            {
                var categoryDto = _mapper.Map<CategoryDto>(category);

                categoriesDto.Add(categoryDto);
            }

            return Ok(categoriesDto);
        }

        /// <summary>
        /// Obtiene la información de una categoría específica utilizando su ID.
        /// </summary>
        /// <param name="categoryId">ID de la categoría a obtener.</param>
        /// <returns>Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación.</returns>
        /// <response code="200">Información de la categoría obtenida exitosamente.</response>
        /// <response code="400">El ID de la categoría es inválido o faltan datos requeridos.</response>
        /// <response code="403">Acceso denegado. Se requiere autorización.</response>
        /// <response code="404">La categoría no fue encontrada.</response>
        /// <remarks>
        /// Este método permite acceso anónimo y utiliza caché para mejorar el rendimiento.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ResponseCache(CacheProfileName = "Default30Seconds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _categoryRepository.GetCategory(categoryId);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

        /// <summary>
        /// Crea una nueva categoría en el sistema.
        /// </summary>
        /// <param name="categoryDto">
        /// Objeto <see cref="CategoryDto"/> que contiene la información necesaria para crear la categoría.
        /// </param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de creación.
        /// </returns>
        /// <response code="201">Categoría creada exitosamente.</response>
        /// <response code="400">El nombre de la categoría ya existe o faltan datos requeridos.</response>
        /// <response code="401">Usuario no autorizado para realizar esta operación.</response>
        /// <response code="500">Error interno del servidor durante el proceso de creación.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// El nombre de la categoría debe ser único en el sistema.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepository.CategoryExists(categoryDto.Name!))
            {
                ModelState.AddModelError("", "La categoría ya existe.");

                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("", "Algo salió mal al guardar la categoría.");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }

        /// <summary>
        /// Actualiza la información de una categoría existente en el sistema.
        /// </summary>
        /// <param name="categoryId">ID de la categoría a actualizar.</param>
        /// <param name="categoryDto">
        /// Objeto <see cref="CategoryDto"/> que contiene la información actualizada de la categoría.
        /// </param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de actualización.
        /// </returns>
        /// <response code="204">Categoría actualizada exitosamente.</response>
        /// <response code="400">Los datos proporcionados son inválidos o el ID no coincide.</response>
        /// <response code="401">Usuario no autorizado para realizar esta operación.</response>
        /// <response code="404">La categoría no fue encontrada.</response>
        /// <response code="500">Error interno del servidor durante el proceso de actualización.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// El ID de la categoría en la URL debe coincidir con el ID en el DTO.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var currentCategory = _categoryRepository.GetCategory(categoryId);

            if (currentCategory == null)
            {
                return NotFound($"No se encontró la categoría con el ID {categoryId}");
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("", "Algo salió mal al actualizar la categoría.");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina una categoría específica del sistema utilizando su ID.
        /// </summary>
        /// <param name="categoryId">ID de la categoría a eliminar.</param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de eliminación.
        /// </returns>
        /// <response code="204">Categoría eliminada exitosamente.</response>
        /// <response code="400">El ID de la categoría es inválido.</response>
        /// <response code="401">Usuario no autorizado para realizar esta operación.</response>
        /// <response code="404">La categoría no fue encontrada.</response>
        /// <response code="500">Error interno del servidor durante el proceso de eliminación.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// Una vez eliminada, la categoría no podrá ser recuperada.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound($"No se encontró la categoría con el ID {categoryId}");
            }

            var category = _categoryRepository.GetCategory(categoryId);

            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "Algo salió mal al eliminar la categoría.");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}