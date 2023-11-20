using System.ComponentModel.DataAnnotations;

namespace ReservaHotel.Modelos
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "El nombre del hotel no puede superar los 255 caracteres.")]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "La ubicación del hotel no puede superar los 255 caracteres.")]
        public string? Ubicacion { get; set; }

        public bool Activo { get; set; }
 

    }
}
