namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa un objeto de transferencia de datos (DTO) para una película, incluyendo información relevante como nombre, descripción, duración, clasificación y categoría.
    /// </summary>
    public class MovieDto
    {
        /// <summary>
        /// Obtiene o establece el identificador único de la película.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la película.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción de la película.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Obtiene o establece la duración de la película en minutos.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Obtiene o establece la ruta de la imagen de la película (URL o ruta remota).
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Obtiene o establece la ruta local de la imagen de la película.
        /// </summary>
        public string? ImageLocalPath { get; set; }

        /// <summary>
        /// Define los tipos de clasificación por edad para una película.
        /// </summary>
        public enum ClassificationType
        {
            /// <summary>
            /// Clasificación para mayores de 7 años.
            /// </summary>
            Seven,
            /// <summary>
            /// Clasificación para mayores de 13 años.
            /// </summary>
            Thirteen,
            /// <summary>
            /// Clasificación para mayores de 16 años.
            /// </summary>
            Sixteen,
            /// <summary>
            /// Clasificación para mayores de 18 años.
            /// </summary>
            Eighteen
        }

        /// <summary>
        /// Obtiene o establece la clasificación por edad de la película.
        /// </summary>
        /// <value>
        /// Un valor de <see cref="ClassificationType"/> que indica la clasificación por edad.
        /// </value>
        public ClassificationType Classification { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de creación del registro de la película.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la categoría a la que pertenece la película.
        /// </summary>
        public int CategoryId { get; set; }
    }
}