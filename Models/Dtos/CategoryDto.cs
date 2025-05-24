using System.ComponentModel.DataAnnotations;

namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa un objeto de transferencia de datos (DTO) para la entidad de categoría.
    /// Contiene información básica sobre una categoría, incluyendo su identificador, nombre y fecha de creación.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la categoría.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// Este campo es obligatorio y no puede exceder los 100 caracteres.
        /// </summary>
        /// <value>
        /// Nombre descriptivo de la categoría.
        /// </value>
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la categoría no puede exceder los 100 caracteres.")]
        public string? Name { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se creó la categoría.
        /// </summary>
        /// <value>
        /// Fecha y hora de creación de la categoría.
        /// </value>
        public DateTime CreatedAt { get; set; }
    }
}