using Microsoft.EntityFrameworkCore;
using ReservaHotel.Entidades;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    // Define DbSet para cada entidad que se mapeará a tablas de la base de datos
    public DbSet<Hotel> Hoteles { get; set; }
    public DbSet<Habitacion> Habitaciones { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
}
