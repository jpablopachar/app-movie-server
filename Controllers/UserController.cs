using System.Net;
using app_movie_server.Models;
using app_movie_server.Models.Dtos;
using app_movie_server.Repositories.Interfaces;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app_movie_server.Controllers
{
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    [ApiVersionNeutral]
    public class UserController(IUserRepository userRepository, IMapper mapper) : ControllerBase
    {
        protected ApiResponse _apiResponse = new();
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30Seconds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            var usersDto = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = _mapper.Map<UserDto>(user);

                usersDto.Add(userDto);
            }

            return Ok(usersDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}", Name = "GetUser")]
        [ResponseCache(CacheProfileName = "Default30Seconds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(string userId)
        {
            var user = _userRepository.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            bool validateUniqueUserName = _userRepository.IsUserUnique(registerDto.UserName!);

            if (!validateUniqueUserName)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;

                _apiResponse.ErrorMessages!.Add("El nombre de usuario ya existe.");

                return BadRequest(_apiResponse);
            }

            var user = await _userRepository.Register(registerDto);

            if (user == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;

                _apiResponse.ErrorMessages!.Add("Error al registrar el usuario.");

                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;

            return Ok(_apiResponse);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _userRepository.Login(loginDto);

            if (response.User == null || string.IsNullOrEmpty(response.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;

                _apiResponse.ErrorMessages!.Add("Error al iniciar sesión.");

                return BadRequest(_apiResponse);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = response;

            return Ok(_apiResponse);
        }
    }
}