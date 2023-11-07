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

        [HttpGet]
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

        [HttpPost]
        public async Task<ActionResult<Habitacion>> CreateHabitacion(Habitacion habitacionModel)
        {
            if (habitacionModel == null)
            {
                return BadRequest("Datos de habitación inválidos.");
            }

            var habitacionEntidad = _mapper.Map<Entidades.Habitacion>(habitacionModel);
            _dbContext.Habitaciones.Add(habitacionEntidad);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Manejar cualquier error de base de datos, por ejemplo, duplicados de clave primaria
                return BadRequest("Error al crear la habitación: " + ex.Message);
            }

            var habitacionCreada = _mapper.Map<Habitacion>(habitacionEntidad);
            return CreatedAtAction("GetHabitacion", new { id = habitacionCreada.Id }, habitacionCreada);
        }
        [HttpPut("{id}")]
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

            // actualización de los valores de la habitación 

            _mapper.Map(habitacionModelo, habitacionEntidad);

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

        [HttpGet("{id}", Name = "ObtenerReserva")]
        public ActionResult<Reserva> ObtenerReserva(int id)
        {
            // buscar la reserva en la base de datos por su ID.
            var reservaEntidad = _dbContext.Reservas.Find(id);

            // Si no se encuentra la reserva, devuelve un resultado NotFound (404).
            if (reservaEntidad == null)
            {
                return NotFound();
            }

            // Mapea la reserva de la entidad a tu modelo de respuesta (Reserva).
            var reservaModel = _mapper.Map<Reserva>(reservaEntidad);

            // Devuelve la reserva encontrada con un resultado OK (200).
            return Ok(reservaModel);
        }
        //private bool HabitacionDisponible(int habitacionId, DateTime fechaEntrada, DateTime fechaSalida)
        //{
        //    // Consulta las reservas existentes que coinciden con la habitación y las fechas
        //    var reservasExistentes = _dbContext.Reservas
        //        .Where(r => r.HabitacionId == habitacionId &&
        //            ((r.FechaSalida >= fechaEntrada && r.FechaEntrada <= fechaEntrada) || (r.FechaEntrada <= fechaSalida && r.FechaSalida >= fechaSalida)))
        //        .ToList();

        //    // Si no hay reservas existentes que se choquen con las fechas proporcionadas, la habitación está disponible
        //    return reservasExistentes.Count == 0;
        //}

        
    }

}
