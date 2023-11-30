using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaHotel.Modelos;
using System.Text.Json;

namespace ReservaHotel.Controllers
{
    [Route("api/hoteles")]
    [ApiController]
    public class HotelesController : ControllerBase
    {
        private readonly HotelDbContext _dbContext;
        private readonly IMapper _mapper;

        public HotelesController(HotelDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

         
        [HttpGet("ListarHoteles")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHoteles()
        {
            var hotelesEntidad = await _dbContext.Hoteles
                .Include(h => h.Habitaciones)
                .ToListAsync();

            var hotelesModelo = _mapper.Map<IEnumerable<ReservaHotel.Modelos.Hotel>>(hotelesEntidad);
            return Ok(hotelesModelo);
        }
         


        [HttpGet("{id}/ListarHotel")]
      
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _dbContext.Hoteles
        .Include(h => h.Habitaciones)
        .FirstOrDefaultAsync(h => h.HotelId == id);

            if (hotel == null)
            {
                return NotFound("Hotel no encontrado.");
            }

            return Ok(hotel);
        }

        [HttpPost("CrearHotel")]
        public async Task<ActionResult<Hotel>> CreateHotel(Hotel hotelModelo)
        {
            
            {
                if (hotelModelo == null)
                {
                    return BadRequest("Datos de hotel inválidos.");
                }

                // Mapea el modelo de vista del hotel a la entidad del hotel.
                var hotelEntidad = _mapper.Map<Entidades.Hotel>(hotelModelo);

                // Agrega el hotel a la base de datos sin habitaciones.
                _dbContext.Hoteles.Add(hotelEntidad);
                await _dbContext.SaveChangesAsync();

               
                var hotelCreado = _mapper.Map<Hotel>(hotelEntidad);

                return CreatedAtAction("GetHotel", new { id = hotelCreado.Id }, hotelCreado);
            }


        }

        [HttpPut("{id}/EditarHotel")]
        public async Task<IActionResult> UpdateHotel(int id, Hotel hotelModelo)
        {
            if (id != hotelModelo.Id)
            {
                return BadRequest("ID del hotel no coincide con los datos proporcionados.");
            }

            var hotelEntidad = await _dbContext.Hoteles.FindAsync(id);

            if (hotelEntidad == null)
            {
                return NotFound("Hotel no encontrado.");
            }

            _mapper.Map(hotelModelo, hotelEntidad);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound("Hotel no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("{id}/HabilitarHotel")]
        public async Task<IActionResult> HabilitarHotel(int id)
        {
            var hotel = await _dbContext.Hoteles.FindAsync(id);

            if (hotel == null)
            {
                return NotFound("Hotel no encontrado.");
            }

            if (hotel.Activo)
            {
                return BadRequest("El hotel ya está habilitado.");
            }

            hotel.Activo = true;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound("Hotel no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("{id}/DeshabilitarHotel")]
        public async Task<IActionResult> DeshabilitarHotel(int id)
        {
            var hotel = await _dbContext.Hoteles.FindAsync(id);

            if (hotel == null)
            {
                return NotFound("Hotel no encontrado.");
            }

            if (!hotel.Activo)
            {
                return BadRequest("El hotel ya está deshabilitado.");
            }

            hotel.Activo = false;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound("Hotel no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // Endpoint para obtener todas las habitaciones disponibles para asignar a un hotel
        [HttpGet("habitaciones-disponibles")]
        public async Task<ActionResult<IEnumerable<Habitacion>>> ObtenerHabitacionesDisponibles()
        {
            var habitacionesDisponibles = await _dbContext.Habitaciones
                .Where(h => h.HotelId == null) // Filtrar habitaciones no asignadas a un hotel
                .ToListAsync();

            var habitacionesModelo = _mapper.Map<List<Habitacion>>(habitacionesDisponibles);
            return habitacionesModelo;
        }

        // Endpoint para asignar una habitación a un hotel
        [HttpPost("{hotelId}/asignar-habitacion/{habitacionId}")]
        public async Task<IActionResult> AsignarHabitacionAHotel(int hotelId, int habitacionId)
        {
            var hotel = await _dbContext.Hoteles.FindAsync(hotelId);
            var habitacion = await _dbContext.Habitaciones.FindAsync(habitacionId);

            if (hotel == null || habitacion == null)
            {
                return NotFound("Hotel o habitación no encontrados.");
            }

            if (habitacion.HotelId == hotelId)
            {
                return BadRequest("La habitación ya está asignada a este hotel.");
            }

            if (habitacion.HotelId != null)
            {
                var otroHotel = await _dbContext.Hoteles.FindAsync(habitacion.HotelId);
                return BadRequest($"La habitación está asignada al hotel '{otroHotel?.Nombre}'.");
            }

            habitacion.HotelId = hotelId;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(hotelId) || !HabitacionExists(habitacionId))
                {
                    return NotFound("Hotel o habitación no encontrados.");
                }
                else
                {
                    throw;
                }
            }

            return Ok($"La habitación fue asignada exitosamente al hotel '{hotel.Nombre}'.");
        }


        private bool HotelExists(int hotelId)
        {
            return _dbContext.Hoteles.Any(h => h.HotelId == hotelId);
        }

        private bool HabitacionExists(int habitacionId)
        {
            return _dbContext.Habitaciones.Any(h => h.Id == habitacionId);
        }

    }
}
