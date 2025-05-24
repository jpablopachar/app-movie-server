using app_movie_server.Models;
using app_movie_server.Models.Dtos;
using app_movie_server.Repositories.Interfaces;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_movie_server.Controllers.V1
{
    [Route("api/v{version:apiVersion}/category")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CategoryController(ICategoryRepository categoryRepository, IMapper mapper) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet("GetString")]
        [Obsolete("Este endpoint está obsoleto. Por favor utilice el endpoint de la versión 2.0.")]
        public IEnumerable<string> Get()
        {
            return ["valor1", "valor2", "valor3"];
        }

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

        [Authorize(Roles = "Admin")]
        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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