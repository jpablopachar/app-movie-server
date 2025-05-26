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
    /// Controlador para gestionar las operaciones relacionadas con las películas en el sistema.
    /// </summary>
    /// <remarks>
    /// Este controlador permite realizar operaciones CRUD sobre las películas,
    /// incluyendo la creación, consulta, actualización, eliminación, búsqueda y filtrado por categorías.
    /// También maneja la carga y gestión de imágenes asociadas a las películas.
    /// </remarks>
    /// <param name="movieRepository">Repositorio de películas para acceder a la base de datos.</param>
    /// <param name="mapper">Instancia de AutoMapper para realizar conversiones entre entidades y DTOs.</param>
    /// <returns>
    /// Una instancia de <see cref="MovieController"/> que permite gestionar las operaciones relacionadas con las películas.
    /// </returns>
    [Route("api/v{version:apiVersion}/movie")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MovieController(IMovieRepository movieRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMovieRepository _movieRepository = movieRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtiene una lista paginada de todas las películas disponibles en el sistema.
        /// </summary>
        /// <param name="pageNumber">Número de página a obtener (por defecto: 1).</param>
        /// <param name="pageSize">Cantidad de películas por página (por defecto: 2).</param>
        /// <returns>Una respuesta <see cref="IActionResult"/> que contiene la lista paginada de películas.</returns>
        /// <response code="200">Lista de películas obtenida exitosamente con información de paginación.</response>
        /// <response code="403">Acceso denegado. Se requiere autorización.</response>
        /// <response code="404">No se encontraron películas.</response>
        /// <response code="500">Error interno del servidor durante el proceso de consulta.</response>
        /// <remarks>
        /// Este método permite acceso anónimo e implementa paginación para mejorar el rendimiento.
        /// La respuesta incluye información de paginación como total de películas, páginas totales, etc.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMovies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            try
            {
                var totalMovies = _movieRepository.GetTotalMovies();
                var movies = _movieRepository.GetMovies(pageNumber, pageSize);

                if (movies == null || movies.Count == 0)
                {
                    return NotFound("No se encontraron películas.");
                }

                var moviesDto = movies.Select(movie => _mapper.Map<MovieDto>(movie)).ToList();

                var response = new
                {
                    TotalMovies = totalMovies,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Movies = moviesDto,
                    TotalPages = (int)Math.Ceiling(totalMovies / (double)pageSize),
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Se ha producido un error en el servidor.");
            }
        }

        /// <summary>
        /// Obtiene la información de una película específica utilizando su ID.
        /// </summary>
        /// <param name="movieId">ID de la película a obtener.</param>
        /// <returns>Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación.</returns>
        /// <response code="200">Información de la película obtenida exitosamente.</response>
        /// <response code="400">El ID de la película es inválido o faltan datos requeridos.</response>
        /// <response code="403">Acceso denegado. Se requiere autorización.</response>
        /// <response code="404">La película no fue encontrada.</response>
        /// <remarks>
        /// Este método permite acceso anónimo y retorna toda la información detallada de la película.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovieById(int movieId)
        {
            var movie = _movieRepository.GetMovie(movieId);

            if (movie == null)
            {
                return NotFound();
            }

            var movieDto = _mapper.Map<MovieDto>(movie);

            return Ok(movieDto);
        }

        /// <summary>
        /// Crea una nueva película en el sistema con la posibilidad de subir una imagen.
        /// </summary>
        /// <param name="createMovieDto">
        /// Objeto <see cref="CreateMovieDto"/> que contiene la información necesaria para crear la película,
        /// incluyendo opcionalmente un archivo de imagen.
        /// </param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de creación.
        /// </returns>
        /// <response code="201">Película creada exitosamente.</response>
        /// <response code="400">El nombre de la película ya existe o faltan datos requeridos.</response>
        /// <response code="401">Usuario no autorizado para realizar esta operación.</response>
        /// <response code="500">Error interno del servidor durante el proceso de creación.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// Si se proporciona una imagen, se guarda en el sistema de archivos y se genera una URL de acceso.
        /// Si no se proporciona imagen, se asigna una imagen placeholder por defecto.
        /// El nombre de la película debe ser único en el sistema.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createMovieDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepository.MovieExists(createMovieDto.Name!))
            {
                ModelState.AddModelError("", "La película ya existe.");

                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(createMovieDto);

            if (createMovieDto.Image != null)
            {
                string fileName = movie.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(createMovieDto.Image.FileName);
                string filaPath = @"wwwroot\images\" + fileName;
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filaPath);
                FileInfo file = new(directoryLocation);

                if (file.Exists)
                {
                    file.Delete();
                }

                using (var stream = new FileStream(directoryLocation, FileMode.Create))
                {
                    createMovieDto.Image.CopyTo(stream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

                movie.ImagePath = $"{baseUrl}/images/{fileName}";

                movie.ImageLocalPath = filaPath;
            }
            else
            {
                movie.ImagePath = "https://placehold.co/600x400";
            }

            _movieRepository.CreateMovie(movie);

            return CreatedAtRoute("", new { movieId = movie.Id }, movie);
        }

        /// <summary>
        /// Actualiza la información de una película existente en el sistema.
        /// </summary>
        /// <param name="movieId">ID de la película a actualizar.</param>
        /// <param name="updateMovieDto">
        /// Objeto <see cref="UpdateMovieDto"/> que contiene la información actualizada de la película,
        /// incluyendo opcionalmente un nuevo archivo de imagen.
        /// </param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de actualización.
        /// </returns>
        /// <response code="204">Película actualizada exitosamente.</response>
        /// <response code="400">Los datos proporcionados son inválidos o el ID no coincide.</response>
        /// <response code="401">Usuario no autorizado para realizar esta operación.</response>
        /// <response code="404">La película no fue encontrada.</response>
        /// <response code="500">Error interno del servidor durante el proceso de actualización.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// El ID de la película en la URL debe coincidir con el ID en el DTO.
        /// Si se proporciona una nueva imagen, reemplaza la imagen anterior.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPut("{movieId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(int movieId, [FromBody] UpdateMovieDto updateMovieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (updateMovieDto == null || movieId != updateMovieDto.Id)
            {
                return BadRequest(ModelState);
            }

            var existingMovie = _movieRepository.GetMovie(movieId);

            if (existingMovie == null)
            {
                return NotFound($"No se encontró la película con el ID {movieId}.");
            }

            var movie = _mapper.Map<Movie>(updateMovieDto);

            if (updateMovieDto.Image != null)
            {
                string fileName = movie.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(updateMovieDto.Image.FileName);
                string filaPath = @"wwwroot\images\" + fileName;
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filaPath);
                FileInfo file = new(directoryLocation);

                if (file.Exists)
                {
                    file.Delete();
                }

                using (var stream = new FileStream(directoryLocation, FileMode.Create))
                {
                    updateMovieDto.Image.CopyTo(stream);
                }

                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

                movie.ImagePath = $"{baseUrl}/images/{fileName}";

                movie.ImageLocalPath = filaPath;
            }
            else
            {
                movie.ImagePath = "https://placehold.co/600x400";
            }

            _movieRepository.UpdateMovie(movie);

            return NoContent();
        }

        /// <summary>
        /// Elimina una película específica del sistema utilizando su ID.
        /// </summary>
        /// <param name="movieId">ID de la película a eliminar.</param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de eliminación.
        /// </returns>
        /// <response code="204">Película eliminada exitosamente.</response>
        /// <response code="400">El ID de la película es inválido.</response>
        /// <response code="401">Usuario no autorizado para realizar esta operación.</response>
        /// <response code="404">La película no fue encontrada.</response>
        /// <response code="500">Error interno del servidor durante el proceso de eliminación.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// Una vez eliminada, la película y sus archivos asociados no podrán ser recuperados.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{movieId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound("");
            }

            var movie = _movieRepository.GetMovie(movieId);

            if (!_movieRepository.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"No se pudo eliminar la película con el ID {movieId}.");

                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Obtiene una lista de todas las películas que pertenecen a una categoría específica.
        /// </summary>
        /// <param name="categoryId">ID de la categoría para filtrar las películas.</param>
        /// <returns>Una respuesta <see cref="IActionResult"/> que contiene la lista de películas de la categoría.</returns>
        /// <response code="200">Lista de películas de la categoría obtenida exitosamente.</response>
        /// <response code="404">No se encontraron películas para esta categoría.</response>
        /// <response code="500">Error interno del servidor durante el proceso de consulta.</response>
        /// <remarks>
        /// Este método permite acceso anónimo y filtra las películas por la categoría especificada.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet("GetMoviesByCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMoviesByCategory(int categoryId)
        {
            try
            {
                var movies = _movieRepository.GetMoviesByCategory(categoryId);

                if (movies == null || movies.Count == 0)
                {
                    return NotFound("No se encontraron películas para esta categoría.");
                }

                var moviesDto = movies.Select(movie => _mapper.Map<MovieDto>(movie)).ToList();

                return Ok(moviesDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Se ha producido un error en el servidor.");
            }
        }

        /// <summary>
        /// Busca películas en el sistema utilizando el nombre como criterio de búsqueda.
        /// </summary>
        /// <param name="name">Nombre o parte del nombre de la película a buscar.</param>
        /// <returns>Una respuesta <see cref="IActionResult"/> que contiene la lista de películas encontradas.</returns>
        /// <response code="200">Búsqueda realizada exitosamente, se devuelven las películas encontradas.</response>
        /// <response code="404">No se encontraron películas con ese nombre.</response>
        /// <response code="500">Error interno del servidor durante el proceso de búsqueda.</response>
        /// <remarks>
        /// Este método permite acceso anónimo y realiza búsqueda parcial por nombre de película.
        /// La búsqueda no distingue entre mayúsculas y minúsculas.
        /// </remarks>
        [AllowAnonymous]
        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchMovies(string name)
        {
            try
            {
                var movies = _movieRepository.SearchMovie(name);

                if (!movies.Any())
                {
                    return NotFound("No se encontraron películas con ese nombre.");
                }

                var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(movies);

                return Ok(moviesDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Se ha producido un error en el servidor.");
            }
        }
    }
}