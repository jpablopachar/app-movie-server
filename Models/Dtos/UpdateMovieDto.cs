namespace app_movie_server.Models.Dtos
{
    /// <summary>
    /// Representa un objeto de transferencia de datos (DTO) utilizado para actualizar la información de una película.
    /// </summary>
    public class UpdateMovieDto
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
        /// Obtiene o establece la ruta de la imagen de la película en el sistema de archivos o almacenamiento externo.
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Obtiene o establece la ruta local de la imagen de la película.
        /// </summary>
        public string? ImageLocalPath { get; set; }

        /// <summary>
        /// Obtiene o establece el archivo de imagen cargado para la película.
        /// </summary>
        /// <remarks>
        /// Este campo se utiliza para la carga de una nueva imagen asociada a la película.
        /// </remarks>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Define los posibles tipos de clasificación por edad para una película.
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
        public ClassificationType Classification { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de la categoría a la que pertenece la película.
        /// </summary>
        public int CategoryId { get; set; }
    }
}