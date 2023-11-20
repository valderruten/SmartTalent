using System;

namespace ReservaHotel.Modelos
{
    public class Reserva
    {
        public int ReservaId { get; set; }
        public int HotelId { get; set; }
        public int HabitacionId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
       
        public string ?ClienteEmail { get; set; }
        public string ?ClienteNombre { get; set; }
        public string? ClienteApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? ClienteGenero { get; set; }
        public string? ClienteTipoDocumento { get; set; }
        public string? ClienteNumeroDocumento { get; set; }
        public string? ClienteTelefonoContacto { get; set; }
        public string? ContactoEmergenciaNombre { get; set; }
        public string? TelefonoEmergenciaTelefono { get; set; }
        public bool EsTitular { get; set; }
    }
}
