using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaHotel.Modelos;
 

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
        [HttpGet]
        public IActionResult ObtenerTodasLasReservas()
        {
            var reservas = _dbContext.Reservas.ToList();
                      
            return Ok(reservas);
        }
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] Reserva reservaInput)
        {
            if (reservaInput == null)
            {
                return BadRequest("Datos de reserva no válidos.");
            }

            // Validación de fechas
            if (reservaInput.FechaEntrada >= reservaInput.FechaSalida)
            {
                return BadRequest("La fecha de entrada debe ser anterior a la fecha de salida.");
            }

            // Validación de disponibilidad de habitaciones
            if (!HabitacionDisponible(reservaInput.HabitacionId, reservaInput.FechaEntrada, reservaInput.FechaSalida))
            {
                return BadRequest("La habitación seleccionada no está disponible para las fechas elegidas.");
            }

            // Asegúrate de que el hotel y la habitación estén activos antes de crear la reserva
            var hotel = await _dbContext.Hoteles.FirstOrDefaultAsync(h => h.Id == reservaInput.HotelId);
            var habitacion = await _dbContext.Habitaciones.FirstOrDefaultAsync(h => h.Id == reservaInput.HabitacionId);

            if (hotel == null || habitacion == null || !hotel.Habilitado || !habitacion.Habilitada)
            {
                return BadRequest("El hotel o la habitación no están disponibles o no existen.");
            }

            var reservaEntidad = new ReservaHotel.Entidades.Reserva
            {
                HotelId = reservaInput.HotelId,
                HabitacionId = reservaInput.HabitacionId,
                FechaEntrada = reservaInput.FechaEntrada,
                FechaSalida = reservaInput.FechaSalida,
                CantidadPersonas = reservaInput.CantidadPersonas,
                ClienteEmail = reservaInput.ClienteEmail,
                ClienteNombre = reservaInput.ClienteNombre,
                ClienteApellido = reservaInput.ClienteApellido,
                ClienteFechaNacimiento = reservaInput.FechaNacimiento,
                ClienteGenero = reservaInput.Genero,
                ClienteTipoDocumento = reservaInput.TipoDocumento,
                ClienteNumeroDocumento = reservaInput.NumeroDocumento,
                ClienteTelefonoContacto = reservaInput.TelefonoContacto,
                ContactoEmergenciaNombre = reservaInput.ContactoEmergenciaNombres,
                ContactoEmergenciaTelefono = reservaInput.ContactoEmergenciaTelefono
            };

            // Añadir la reserva a la base de datos.
            _dbContext.Reservas.Add(reservaEntidad);
            await _dbContext.SaveChangesAsync();

            return NoContent(); // Respuesta 204 (No Content) ya que no se retorna la reserva creada.
        }

        private bool HabitacionDisponible(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida)
        {
            // verificación de disponibilidad
            var reservasExistentes = _dbContext.Reservas
                .Where(r => r.HabitacionId == habitacionId &&
                            !(r.FechaSalida <= fechaEntrada || r.FechaEntrada >= fechaSalida))
                .ToList();


            return reservasExistentes.Count == 0;
        }


    }
}
