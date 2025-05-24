namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa un objeto de transferencia de datos (DTO) para un usuario en el sistema.
    /// Contiene información básica del usuario necesaria para operaciones de autenticación y autorización.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario utilizado para iniciar sesión en el sistema.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// <para>
        /// <b>Advertencia:</b> Por razones de seguridad, se recomienda no exponer ni almacenar contraseñas en texto plano.
        /// </para>
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Obtiene o establece el rol asignado al usuario dentro del sistema.
        /// </summary>
        public string? Role { get; set; }
    }
}