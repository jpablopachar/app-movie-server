using System.ComponentModel.DataAnnotations;

namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa un objeto de transferencia de datos (DTO) utilizado para el inicio de sesión de un usuario.
    /// Contiene las credenciales necesarias para autenticar al usuario en el sistema.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Obtiene o establece el nombre de usuario utilizado para el inicio de sesión.
        /// Este campo es obligatorio.
        /// </summary>
        /// <value>
        /// Nombre de usuario del usuario que intenta iniciar sesión.
        /// </value>
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string? UserName { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña asociada al usuario.
        /// Este campo es obligatorio.
        /// </summary>
        /// <value>
        /// Contraseña del usuario que intenta iniciar sesión.
        /// </value>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Password { get; set; }
    }
}