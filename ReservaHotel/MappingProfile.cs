using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo de la entidad Hotel a su correspondiente modelo
        CreateMap<ReservaHotel.Entidades.Hotel, ReservaHotel.Modelos.Hotel>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            //.ForMember(dest => dest.Habitaciones, opt => opt.MapFrom(src => src.Habitaciones))
            .ForMember(dest => dest.Habilitado, opt => opt.MapFrom(src => src.Habilitado))
            .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion));

        // Mapeo de la entidad Habitacion a su correspondiente modelo
        CreateMap<ReservaHotel.Entidades.Habitacion, ReservaHotel.Modelos.Habitacion>()
            //.ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
            .ForMember(dest => dest.CostoBase, opt => opt.MapFrom(src => src.CostoBase))
            .ForMember(dest => dest.Impuestos, opt => opt.MapFrom(src => src.Impuestos))
            .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion))
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
            .ForMember(dest => dest.Habilitada, opt => opt.MapFrom(src => src.Habilitada));
        CreateMap<ReservaHotel.Entidades.Habitacion, ReservaHotel.Modelos.Habitacion>();
        CreateMap<ReservaHotel.Modelos.Habitacion, ReservaHotel.Entidades.Habitacion>();
        CreateMap<ReservaHotel.Entidades.Hotel, ReservaHotel.Modelos.Hotel>();
        CreateMap<ReservaHotel.Modelos.Hotel, ReservaHotel.Entidades.Hotel>();

        // Mapeo entre ReservaHotel.Modelos.Reserva y ReservaHotel.Entidades.Reserva
        CreateMap<ReservaHotel.Modelos.Reserva, ReservaHotel.Entidades.Reserva>();
        CreateMap<ReservaHotel.Entidades.Reserva, ReservaHotel.Modelos.Reserva>();

    }
}


