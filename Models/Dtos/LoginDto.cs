using System.ComponentModel.DataAnnotations;

namespace app_movie_server.Models.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string? Password { get; set; }
    }
}