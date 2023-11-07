namespace ReservaHotel.Modelos
{
    public class Habitacion
    {
        public int Id { get; set; }

        public int HotelId { get; set; }
        
        public decimal CostoBase { get; set; }
        public decimal Impuestos { get; set; }
        public string? TipoHabitacion { get; set; }
        
        public string? Ubicacion { get; set; }
                
        public bool Habilitada { get; set; }
    }
}
