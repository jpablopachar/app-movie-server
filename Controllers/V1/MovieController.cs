using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}