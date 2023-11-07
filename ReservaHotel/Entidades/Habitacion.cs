
using ReservaHotel.Modelos;
using System.ComponentModel.DataAnnotations;

namespace ReservaHotel.Entidades
{

    public class Habitacion
    {
        public int Id { get; set; }

        public int HotelId { get; set; }
         

        [Required]
        public decimal CostoBase { get; set; }

        [Required]
        public decimal Impuestos { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "La ubicación de la habitación no puede superar los 100 caracteres.")]
        public string ?Ubicacion { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Tipo de habitación no puede superar los 100 caracteres.")]
        public string? TipoHabitacion { get; set; }

        public bool Habilitada { get; set; }

        public virtual Hotel? Hotel { get; set; }
    }
}
