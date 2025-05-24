using System.ComponentModel.DataAnnotations;

namespace app_movie_server.Models.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la categoría no puede exceder los 100 caracteres.")]
        public string? Name { get; set; }
    }
}