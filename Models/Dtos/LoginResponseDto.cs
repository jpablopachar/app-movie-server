namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa la respuesta devuelta tras un intento de inicio de sesión exitoso.
    /// Contiene la información del usuario autenticado, su rol y el token de autenticación.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Obtiene o establece los datos del usuario autenticado.
        /// </summary>
        /// <value>
        /// Un objeto <see cref="UserDataDto"/> que contiene la información del usuario, o <c>null</c> si no está disponible.
        /// </value>
        public UserDataDto? User { get; set; }

        /// <summary>
        /// Obtiene o establece el rol asignado al usuario autenticado.
        /// </summary>
        /// <value>
        /// Una cadena que representa el rol del usuario, o <c>null</c> si no se ha asignado ningún rol.
        /// </value>
        public string? Role { get; set; }

        /// <summary>
        /// Obtiene o establece el token de autenticación generado para el usuario.
        /// </summary>
        /// <value>
        /// Una cadena que contiene el token JWT o similar, o <c>null</c> si no se ha generado ningún token.
        /// </value>
        public string? Token { get; set; }
    }
}