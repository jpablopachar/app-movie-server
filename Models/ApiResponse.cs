using System.Net;

namespace app_movie_server.Models
{
    /// <summary>
    /// Representa una respuesta estándar para las operaciones de la API, incluyendo el código de estado, el resultado, y mensajes de error.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Obtiene o establece el código de estado HTTP asociado a la respuesta.
        /// </summary>
        /// <value>
        /// Código de estado HTTP de la respuesta. Puede ser nulo si no se ha especificado.
        /// </value>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la operación fue exitosa.
        /// </summary>
        /// <value>
        /// <c>true</c> si la operación fue exitosa; en caso contrario, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// Obtiene o establece la lista de mensajes de error asociados a la respuesta.
        /// </summary>
        /// <value>
        /// Lista de mensajes de error. Puede ser nula si no existen errores.
        /// </value>
        public List<string>? ErrorMessages { get; set; }

        /// <summary>
        /// Obtiene o establece el resultado devuelto por la operación de la API.
        /// </summary>
        /// <value>
        /// Objeto que representa el resultado de la operación. Puede ser nulo si no hay resultado.
        /// </value>
        public object? Result { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ApiResponse"/>.
        /// </summary>
        /// <remarks>
        /// El constructor inicializa la lista de mensajes de error como una lista vacía.
        /// </remarks>
        public ApiResponse()
        {
            ErrorMessages = [];
        }
    }
}