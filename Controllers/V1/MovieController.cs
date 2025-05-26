using app_movie_server.Models;
using app_movie_server.Models.Dtos;
using app_movie_server.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_movie_server.Controllers.V1
{
    public class MovieController(IMovieRepository movieRepository, IMapper mapper) : ControllerBase
    {
        private readonly IMovieRepository _movieRepository = movieRepository;
        private readonly IMapper _mapper = mapper;

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [AllowAnonymous]
        [HttpGet("{movieId:int}")]
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{movieId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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