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
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los usuarios en el sistema.
    /// </summary>
    /// <remarks>
    /// Este controlador permite realizar operaciones como registrar nuevos usuarios, iniciar sesión,
    /// y gestionar la información de los usuarios existentes.
    /// </remarks>
    /// <param name="userRepository">Repositorio de usuarios para acceder a la base de datos.</param>
    /// <param name="mapper">Instancia de AutoMapper para realizar conversiones entre entidades y DTOs.</param>
    /// <returns>
    /// Una instancia de <see cref="UserController"/> que permite gestionar las operaciones relacionadas con los usuarios.
    /// </returns>
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    [ApiVersionNeutral]
    public class UserController(IUserRepository userRepository, IMapper mapper) : ControllerBase
    {
        protected ApiResponse _apiResponse = new();
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Obtiene una lista de todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>Una respuesta <see cref="IActionResult"/> que contiene la lista de usuarios.</returns>
        /// <response code="200">Lista de usuarios obtenida exitosamente.</response>
        /// <response code="403">Acceso denegado. Se requiere autorización.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// </remarks>
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

        /// <summary>
        /// Obtiene la información de un usuario específico utilizando su ID.
        /// </summary>
        /// <param name="userId">ID del usuario a obtener.</param>
        /// <returns>Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación.</returns>
        /// <response code="200">Información del usuario obtenida exitosamente.</response>
        /// <response code="400">El ID del usuario es inválido o faltan datos requeridos.</response>
        /// <response code="403">Acceso denegado. Se requiere autorización.</response>
        /// <response code="404">El usuario no fue encontrado.</response>
        /// <remarks>
        /// Este método requiere autorización y solo puede ser accedido por usuarios con el rol de "Admin".
        /// </remarks>
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

        /// <summary>
        /// Registra un nuevo usuario en el sistema utilizando las credenciales proporcionadas.
        /// </summary>
        /// <param name="registerDto">
        /// Objeto <see cref="RegisterDto"/> que contiene la información necesaria para registrar el usuario.
        /// </param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de registro.
        /// </returns>
        /// <response code="201">Registro exitoso del nuevo usuario.</response>
        /// <response code="400">El nombre de usuario ya existe o faltan datos requeridos.</response>
        /// <response code="500">Error interno del servidor durante el proceso de registro.</response>
        /// <remarks>
        /// Este método permite el acceso anónimo. Valida que el nombre de usuario sea único antes de proceder con el registro.
        /// </remarks>
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

        /// <summary>
        /// Inicia sesión de un usuario utilizando las credenciales proporcionadas.
        /// </summary>
        /// <param name="loginDto">
        /// Objeto <see cref="LoginDto"/> que contiene las credenciales del usuario para autenticación.
        /// </param>
        /// <returns>
        /// Una respuesta <see cref="IActionResult"/> que indica el resultado de la operación de inicio de sesión.
        /// Devuelve <see cref="OkObjectResult"/> con la información del usuario y el token si la autenticación es exitosa,
        /// o <see cref="BadRequestObjectResult"/> si las credenciales son inválidas.
        /// </returns>
        /// <response code="200">Inicio de sesión exitoso. Se devuelve el usuario autenticado y el token.</response>
        /// <response code="400">Las credenciales proporcionadas son inválidas o faltan datos requeridos.</response>
        /// <response code="500">Error interno del servidor durante el proceso de autenticación.</response>
        /// <remarks>
        /// Este método permite a usuarios anónimos autenticarse en el sistema. Si las credenciales son correctas,
        /// se genera un token de autenticación y se retorna junto con la información del usuario.
        /// </remarks>
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