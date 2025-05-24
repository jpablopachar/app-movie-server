using System.ComponentModel.DataAnnotations;

namespace app_movie_server.Models
{
    /// <summary>
    /// Representa una categoría utilizada para clasificar elementos dentro de la aplicación.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la categoría.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// </summary>
        /// <value>
        /// Nombre descriptivo de la categoría. Este valor es obligatorio.
        /// </value>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de creación de la categoría.
        /// </summary>
        /// <value>
        /// Fecha y hora en que la categoría fue creada. Este valor es obligatorio.
        /// </value>
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}