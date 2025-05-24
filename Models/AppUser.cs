using Microsoft.AspNetCore.Identity;

namespace app_movie_server.Models
{
    /// <summary>
    /// Representa un usuario de la aplicación, extendiendo la funcionalidad proporcionada por <see cref="IdentityUser"/>.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Obtiene o establece el nombre completo del usuario de la aplicación.
        /// </summary>
        /// <value>
        /// Una cadena que contiene el nombre del usuario. Puede ser nulo si no se ha proporcionado.
        /// </value>
        public string? Name { get; set; }
    }
}