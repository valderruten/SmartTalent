using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ReservaHotel.Entidades
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "El nombre del hotel no puede superar los 255 caracteres.")]
        public string ?Nombre { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "La ubicación del hotel no puede superar los 255 caracteres.")]
        public string ?Ubicacion { get; set; }

        public bool Habilitado { get; set; }

        public virtual ICollection<Habitacion>?Habitaciones { get; set; }
    }
}
