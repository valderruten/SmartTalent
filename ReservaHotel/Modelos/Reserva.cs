using System;

namespace ReservaHotel.Modelos
{
    public class Reserva
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int HabitacionId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int CantidadPersonas { get; set; }
        
       // public Habitacion Habitacion { get; set; }
        public string ?ClienteEmail { get; set; }
        public string ?ClienteNombre { get; set; }
        public string? ClienteApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Genero { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? ContactoEmergenciaNombres { get; set; }
        public string? ContactoEmergenciaTelefono { get; set; }
    }
}
