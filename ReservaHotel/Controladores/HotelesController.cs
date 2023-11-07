using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaHotel.Modelos;

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

         
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHoteles()
        {
            var hotelesEntidad = await _dbContext.Hoteles
                .Include(h => h.Habitaciones) // Esto incluirá las habitaciones relacionadas en la consulta
                .ToListAsync();

            var hotelesModelo = _mapper.Map<IEnumerable<Hotel>>(hotelesEntidad);
            return Ok(hotelesModelo);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _dbContext.Hoteles
        .Include(h => h.Habitaciones)
        .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
            {
                return NotFound("Hotel no encontrado.");
            }

            return Ok(hotel);
        }

        [HttpPost]
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

        [HttpPut("{id}")]
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

        [HttpPut("{id}/habilitar")]
        public async Task<IActionResult> ToggleHotelStatus(int id)
        {
            var hotel = await _dbContext.Hoteles.FindAsync(id);

            if (hotel == null)
            {
                return NotFound("Hotel no encontrado.");
            }

            hotel.Habilitado = !hotel.Habilitado;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
         

        private bool HotelExists(int id)
        {
            return _dbContext.Hoteles.Any(e => e.Id == id);
        }
    }
}
