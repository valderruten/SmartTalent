using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaHotel.Modelos;

namespace ReservaHotel.Controllers
{
    [Route("api/habitaciones")]
    [ApiController]
    public class HabitacionesController : ControllerBase
    {
        private readonly HotelDbContext _dbContext;
        private readonly IMapper _mapper;  

        public HabitacionesController(HotelDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("listarHabitaciones")]
        public async Task<ActionResult<IEnumerable<Habitacion>>> GetHabitaciones()

        {
            var habitacionesEntidad = await _dbContext.Habitaciones.ToListAsync();
            var habitacionesModelo = _mapper.Map<List<Habitacion>>(habitacionesEntidad);
            return habitacionesModelo;
        }
        private bool HabitacionExists(int habitacionId)
        {
            return _dbContext.Habitaciones.Any(h => h.Id == habitacionId);
        }
        [HttpPost("CrearHabitacion")]
        public async Task<ActionResult<Habitacion>> CreateHabitacion(Habitacion habitacionModel)
        {
            if (habitacionModel == null)
            {
                return BadRequest("Datos de habitación inválidos.");
            }

            try
            {
                
                    habitacionModel.HotelId = null; // Asignar null al HotelId para indicar que no está vinculada a un hotel
                habitacionModel.EstaOcupada = false; // Por defecto, la habitación no está ocupada
                var habitacionEntidad = _mapper.Map<Entidades.Habitacion>(habitacionModel);
               habitacionEntidad.CapacidadPersonas = habitacionModel.CapacidadPersonas;

                _dbContext.Habitaciones.Add(habitacionEntidad);
                await _dbContext.SaveChangesAsync();

                var habitacionCreada = _mapper.Map<Habitacion>(habitacionEntidad);
                return CreatedAtAction("GetHabitacion", new { id = habitacionCreada.Id }, habitacionCreada);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Error al crear la habitación: " + ex.InnerException?.Message);
            }
        }
       


        [HttpPut("{id}/EditarHabitacion")]
        public async Task<IActionResult> UpdateHabitacion(int id, [FromBody] Habitacion habitacionModelo)
        {
            if (id != habitacionModelo.Id)
            {
                return BadRequest("ID de habitación no coincide con los datos proporcionados.");
            }

            var habitacionEntidad = await _dbContext.Habitaciones.FindAsync(id);

            if (habitacionEntidad == null)
            {
                return NotFound("Habitación no encontrada.");
            }

        
            habitacionEntidad.CostoBase = habitacionModelo.CostoBase;
            habitacionEntidad.Impuestos = habitacionModelo.Impuestos;
            habitacionEntidad.TipoHabitacion = habitacionModelo.TipoHabitacion;
            habitacionEntidad.Ubicacion = habitacionModelo.Ubicacion;
            habitacionEntidad.CapacidadPersonas = habitacionModelo.CapacidadPersonas;
            habitacionEntidad.Activo = habitacionModelo.Activo;
            habitacionEntidad.EstaOcupada = habitacionModelo.EstaOcupada;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacionExists(id))
                {
                    return NotFound("Habitación no encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


       
       private bool HabitacionDisponible(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida)
        {
            // Consulta las reservas existentes que coinciden con la habitación y las fechas
            var reservasExistentes = _dbContext.Reservas
                .Where(r => r.HabitacionId == habitacionId &&
                    ((r.FechaSalida >= fechaEntrada && r.FechaEntrada <= fechaEntrada) || (r.FechaEntrada <= fechaSalida && r.FechaSalida >= fechaSalida)))
                .ToList();

            // Si no hay reservas existentes que se choquen con las fechas proporcionadas, la habitación está disponible
            return reservasExistentes.Count == 0;
        }

        [HttpPut("{id}/habilitarHabitacion")]
        public async Task<IActionResult> HabilitarHabitacion(int id)
        {
            var habitacionEntidad = await _dbContext.Habitaciones.FindAsync(id);

            if (habitacionEntidad == null)
            {
                return NotFound("Habitación no encontrada.");
            }

            if (habitacionEntidad.Activo)
            {
                return BadRequest("La habitación ya está habilitada.");
            }

            habitacionEntidad.Activo = true; // Para habilitar la habitación

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Habitación habilitada exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al habilitar la habitación: {ex.Message}");
            }
        }

        [HttpPut("{id}/deshabilitarHabitacion")]
        public async Task<IActionResult> DeshabilitarHabitacion(int id)
        {
            var habitacionEntidad = await _dbContext.Habitaciones.FindAsync(id);

            if (habitacionEntidad == null)
            {
                return NotFound("Habitación no encontrada.");
            }

            if (!habitacionEntidad.Activo)
            {
                return BadRequest("La habitación ya está deshabilitada.");
            }

            habitacionEntidad.Activo = false; // Para deshabilitar la habitación

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Habitación deshabilitada exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al deshabilitar la habitación: {ex.Message}");
            }
        }

        [HttpGet("{id}/ObtenerReserva", Name = "ObtenerReserva")]
        public ActionResult<Reserva> ObtenerReserva(int id)
        {
            // buscar la reserva en la base de datos por su ID.
            var reservaEntidad = _dbContext.Reservas.Find(id);

            // Si no se encuentra la reserva, devuelve un resultado NotFound (404).
            if (reservaEntidad == null)
            {
                return NotFound();
            }

             
            var reservaModel = _mapper.Map<Reserva>(reservaEntidad);

            // Devuelve la reserva encontrada con un resultado OK (200).
            return Ok(reservaModel);
        }
        
        [HttpGet("{id}", Name = "GetHabitacion")]
        public async Task<ActionResult<Habitacion>> GetHabitacion(int id)
        {
            var habitacion = await _dbContext.Habitaciones.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound("Habitación no encontrada.");
            }
            return Ok(habitacion);
        }

    }

}
