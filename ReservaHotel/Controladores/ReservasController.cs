using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaHotel.Entidades;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReservaHotel.Controllers
{
    [Route("api/reservas")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly HotelDbContext _dbContext;

        public ReservasController(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("MostrarReservas")]
        public IActionResult ObtenerTodasLasReservas()
        {
            var reservas = _dbContext.Reservas.ToList();
            return Ok(reservas);
        }

        [HttpPost("CrearReserva")]
        public async Task<IActionResult> CrearReserva([FromBody] ReservaInput reservaInput)
        {
            if (reservaInput == null)
            {
                return BadRequest("Datos de reserva no válidos.");
            }

            if (reservaInput.FechaEntrada >= reservaInput.FechaSalida)
            {
                return BadRequest("La fecha de entrada debe ser anterior a la fecha de salida.");
            }

            var disponibilidadHabitacion = HabitacionDisponible(reservaInput.HabitacionId, reservaInput.FechaEntrada, reservaInput.FechaSalida);
            if (disponibilidadHabitacion != "La habitación está disponible.")
            {
                return BadRequest(disponibilidadHabitacion);
            }

            var habitacion = await _dbContext.Habitaciones.FirstOrDefaultAsync(h => h.Id == reservaInput.HabitacionId && h.HotelId == reservaInput.HotelId);

            if (habitacion == null)
            {
                var habitacionAsignada = await _dbContext.Habitaciones.FindAsync(reservaInput.HabitacionId);
                var hotelAsignado = await _dbContext.Hoteles.FindAsync(habitacionAsignada?.HotelId);

                return BadRequest($"La habitación está asignada al hotel '{hotelAsignado?.Nombre}'.");
            }

            var hotel = await _dbContext.Hoteles.FirstOrDefaultAsync(h => h.HotelId == reservaInput.HotelId);
            if (hotel == null || !hotel.Activo)
            {
                return BadRequest("El hotel no está disponible o no existe.");
            }

            if (reservaInput.EsTitular && reservaInput.CantidadPersonas <= 0)
            {
                return BadRequest("Si es titular, debe especificar una cantidad de personas mayor a 0.");
            }

            if (!reservaInput.EsTitular && reservaInput.CantidadPersonas != 1)
            {
                reservaInput.CantidadPersonas = 1;
            }

            var reservasEnEsaHabitacion = await _dbContext.Reservas
                .Where(r => r.HabitacionId == reservaInput.HabitacionId &&
                            r.FechaEntrada < reservaInput.FechaSalida &&
                            r.FechaSalida > reservaInput.FechaEntrada)
                .ToListAsync();

            int cantidadPersonasReservadas = reservasEnEsaHabitacion.Sum(r => r.CantidadPersonas);
            int totalPersonas = cantidadPersonasReservadas + reservaInput.CantidadPersonas;

            if (totalPersonas > habitacion.CapacidadPersonas)
            {
                return BadRequest($"La habitación permite un máximo de '{habitacion.CapacidadPersonas}' personas.");
            }

            if (reservasEnEsaHabitacion.Count == 0)
            {
                habitacion.EstaOcupada = true;
                await _dbContext.SaveChangesAsync();
            }

            var reservaEntidad = new Reserva
            {
                HotelId = reservaInput.HotelId,
                HabitacionId = reservaInput.HabitacionId,
                FechaReserva = reservaInput.FechaReserva,
                FechaEntrada = reservaInput.FechaEntrada,
                FechaSalida = reservaInput.FechaSalida,
                ClienteNombre = reservaInput.ClienteNombre,
                ClienteApellido = reservaInput.ClienteApellido,
                ClienteGenero = reservaInput.ClienteGenero,
                ClienteFechaNacimiento = reservaInput.ClienteFechaNacimiento,
                ClienteTipoDocumento = reservaInput.ClienteTipoDocumento,
                ClienteNumeroDocumento = reservaInput.ClienteNumeroDocumento,
                ClienteEmail = reservaInput.ClienteEmail,
                ClienteTelefonoContacto = reservaInput.ClienteTelefonoContacto,
                ContactoEmergenciaNombre = reservaInput.ContactoEmergenciaNombre,
                TelefonoEmergenciaTelefono = reservaInput.TelefonoEmergenciaTelefono,
                EsTitular = reservaInput.EsTitular,
                CantidadPersonas = reservaInput.CantidadPersonas
            };

            _dbContext.Reservas.Add(reservaEntidad);
            await _dbContext.SaveChangesAsync();

            return Ok("Reserva realizada con éxito.");
        }

        private string HabitacionDisponible(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida)
        {
            var habitacion = _dbContext.Habitaciones
                .FirstOrDefault(h => h.Id == habitacionId && h.EstaOcupada);

            if (habitacion != null)
            {
                var reservasExistentes = _dbContext.Reservas
                    .Where(r => r.HabitacionId == habitacionId &&
                                !(r.FechaSalida <= fechaEntrada || r.FechaEntrada >= fechaSalida))
                    .ToList();

                int cantidadPersonasReservadas = reservasExistentes.Sum(r => r.CantidadPersonas);
                int totalPersonas = cantidadPersonasReservadas + 1; // Se suma 1 por la nueva reserva

                if (totalPersonas > habitacion.CapacidadPersonas)
                {
                    return "Lo sentimos, la habitación ya está llena para las fechas seleccionadas.";
                }
            }

            return "La habitación está disponible.";
        }
    }

    public class ReservaInput
    {
        public int HotelId { get; set; }
        public int HabitacionId { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ClienteApellido { get; set; }
        public string? ClienteGenero { get; set; }
        public DateTime ClienteFechaNacimiento { get; set; }
        public string? ClienteTipoDocumento { get; set; }
        public string? ClienteNumeroDocumento { get; set; }
        public string? ClienteEmail { get; set; }
        public string? ClienteTelefonoContacto { get; set; }
        public string? ContactoEmergenciaNombre { get; set; }
        public string? TelefonoEmergenciaTelefono { get; set; }
        public bool EsTitular { get; set; }
        public int CantidadPersonas { get; set; }
    }
}
