using System;
using System.ComponentModel.DataAnnotations;

namespace ReservaHotel.Entidades
{
    public class Reserva
    {
        public int ReservaId { get; set; }

        public int HotelId { get; set; }

        public int HabitacionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaReserva { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ClienteFechaNacimiento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaEntrada { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaSalida { get; set; }

       [Required]
       [Range(1, int.MaxValue, ErrorMessage = "La cantidad de personas debe ser al menos 1.")]
       public int CantidadPersonas { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico del cliente no es válido.")]
        public string ?ClienteEmail { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El nombre del cliente no puede superar los 100 caracteres.")]
        public string ?ClienteNombre { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El apellido del cliente no puede superar los 100 caracteres.")]
        public string ?ClienteApellido { get; set; }

       

        [Required]
        [MaxLength(10, ErrorMessage = "El género del cliente no puede superar los 10 caracteres.")]
        public string ?ClienteGenero { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "El tipo de documento del cliente no puede superar los 20 caracteres.")]
        public string ?ClienteTipoDocumento { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "El número de documento del cliente no puede superar los 20 caracteres.")]
        public string ?ClienteNumeroDocumento { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "El teléfono del cliente no puede superar los 15 caracteres.")]
        public string ?ClienteTelefonoContacto { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El nombre del contacto de emergencia no puede superar los 100 caracteres.")]
        public string ?ContactoEmergenciaNombre { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "El teléfono del contacto de emergencia no puede superar los 15 caracteres.")]
        public string ?TelefonoEmergenciaTelefono { get; set; }

        public virtual Habitacion ?Habitacion { get; set; }
        public bool EsTitular { get; set; }
    }
}

