namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa un objeto de transferencia de datos (DTO) que contiene información básica del usuario.
    /// </summary>
    public class UserDataDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        public string? ID { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario utilizado para autenticación o identificación.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string? Name { get; set; }
    }
}