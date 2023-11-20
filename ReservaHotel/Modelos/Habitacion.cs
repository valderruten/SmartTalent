using System.ComponentModel.DataAnnotations;

namespace ReservaHotel.Modelos
{
    public class Habitacion
    {
        public int Id { get; set; }

        public int? HotelId { get; set; }

        [Required(ErrorMessage = "El campo CostoBase es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo CostoBase debe ser mayor o igual a cero.")]
        public decimal CostoBase { get; set; }

        [Required(ErrorMessage = "El campo Impuestos es requerido.")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo Impuestos debe ser mayor o igual a cero.")]
        public decimal Impuestos { get; set; }

        [Required(ErrorMessage = "El campo TipoHabitacion es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo TipoHabitacion no puede tener más de 50 caracteres.")]
        public string ?TipoHabitacion { get; set; }

        [Required(ErrorMessage = "El campo Ubicacion es requerido.")]
        [MaxLength(100, ErrorMessage = "El campo Ubicacion no puede tener más de 100 caracteres.")]
        public string ?Ubicacion { get; set; }

        public bool Activo { get; set; }

        [Required(ErrorMessage = "El campo CapacidadPersonas es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "La Capacidad de Personas debe ser mayor que cero.")]
        public int CapacidadPersonas { get; set; }
        public bool EstaOcupada { get; set; }
    }
}
