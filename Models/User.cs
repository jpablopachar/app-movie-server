using System.ComponentModel.DataAnnotations;

namespace app_movie_server.Models
{
    /// <summary>
    /// Representa a un usuario dentro del sistema.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre completo del usuario.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de usuario utilizado para autenticación.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// </summary>
        /// <remarks>
        /// Por razones de seguridad, se recomienda almacenar la contraseña de forma cifrada.
        /// </remarks>
        public string? Password { get; set; }

        /// <summary>
        /// Obtiene o establece el rol asignado al usuario dentro del sistema.
        /// </summary>
        /// <remarks>
        /// El rol determina los permisos y accesos del usuario.
        /// </remarks>
        public string? Role { get; set; }
    }
}