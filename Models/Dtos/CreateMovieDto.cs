namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa un objeto de transferencia de datos para la creación de una película.
    /// </summary>
    public class CreateMovieDto
    {
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
        /// Obtiene o establece la ruta de la imagen asociada a la película.
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Obtiene o establece el archivo de imagen cargado para la película.
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Define los tipos de clasificación disponibles para la creación de una película.
        /// </summary>
        public enum CreateClassificationType
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
        /// Obtiene o establece la clasificación de la película.
        /// </summary>
        public CreateClassificationType Classification { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la categoría a la que pertenece la película.
        /// </summary>
        public int CategoryId { get; set; }
    }
}