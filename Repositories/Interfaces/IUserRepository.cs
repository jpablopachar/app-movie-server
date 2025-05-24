using app_movie_server.Models;
using app_movie_server.Models.Dtos;

namespace app_movie_server.Repositories.Interfaces
{
    /// <summary>
    /// Define la interfaz para el repositorio de usuarios, proporcionando métodos para la gestión y autenticación de usuarios en el sistema.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Obtiene una colección de todos los usuarios registrados en el sistema.
        /// </summary>
        /// <returns>
        /// Una colección de objetos <see cref="AppUser"/> que representan a los usuarios.
        /// </returns>
        ICollection<AppUser> GetUsers();

        /// <summary>
        /// Recupera un usuario específico a partir de su identificador único.
        /// </summary>
        /// <param name="userId">Identificador único del usuario a recuperar.</param>
        /// <returns>
        /// El objeto <see cref="AppUser"/> correspondiente al identificador proporcionado, o <c>null</c> si no se encuentra.
        /// </returns>
        AppUser GetUser(string userId);

        /// <summary>
        /// Verifica si el identificador de usuario proporcionado es único en el sistema.
        /// </summary>
        /// <param name="userId">Identificador de usuario a verificar.</param>
        /// <returns>
        /// <c>true</c> si el usuario es único; de lo contrario, <c>false</c>.
        /// </returns>
        bool IsUserUnique(string userId);

        /// <summary>
        /// Realiza el proceso de autenticación de un usuario utilizando las credenciales proporcionadas.
        /// </summary>
        /// <param name="loginDto">Objeto que contiene las credenciales de inicio de sesión del usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado contiene un objeto <see cref="LoginResponseDto"/> con la información de autenticación.
        /// </returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Se produce si las credenciales proporcionadas son inválidas.
        /// </exception>
        Task<LoginResponseDto> Login(LoginDto loginDto);

        /// <summary>
        /// Registra un nuevo usuario en el sistema con la información proporcionada.
        /// </summary>
        /// <param name="registerDto">Objeto que contiene los datos necesarios para el registro del usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica. El resultado contiene un objeto <see cref="UserDataDto"/> con los datos del usuario registrado.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Se produce si los datos de registro son inválidos o incompletos.
        /// </exception>
        Task<UserDataDto> Register(RegisterDto registerDto);
    }
}