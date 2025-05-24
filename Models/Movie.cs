using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_movie_server.Models
{
    /// <summary>
    /// Representa una película dentro del sistema.
    /// Contiene información relevante como nombre, descripción, duración, clasificación, fechas y categoría asociada.
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Identificador único de la película.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la película.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descripción detallada de la película.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Duración de la película en minutos.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Ruta de la imagen asociada a la película (almacenamiento externo o URL).
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Ruta local de la imagen asociada a la película (en el sistema de archivos).
        /// </summary>
        public string? ImageLocalPath { get; set; }

        /// <summary>
        /// Enumeración que define los tipos de clasificación por edad para la película.
        /// </summary>
        public enum ClassificationType
        {
            /// <summary>
            /// Apta para mayores de 7 años.
            /// </summary>
            Seven,
            /// <summary>
            /// Apta para mayores de 13 años.
            /// </summary>
            Thirteen,
            /// <summary>
            /// Apta para mayores de 16 años.
            /// </summary>
            Sixteen,
            /// <summary>
            /// Apta para mayores de 18 años.
            /// </summary>
            Eighteen
        }

        /// <summary>
        /// Fecha y hora de creación del registro de la película.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Identificador de la categoría a la que pertenece la película.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Categoría asociada a la película.
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}