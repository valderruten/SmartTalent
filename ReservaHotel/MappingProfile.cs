using AutoMapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ReservaHotel.Entidades.Hotel, ReservaHotel.Modelos.Hotel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.HotelId)); // Mapear HotelId a Id

        CreateMap<ReservaHotel.Modelos.Hotel, ReservaHotel.Entidades.Hotel>()
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Id)); // Mapear Id a HotelId

        CreateMap<ReservaHotel.Entidades.Habitacion, ReservaHotel.Modelos.Habitacion>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Mapear Id a Id
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId)); // Mapear HotelId a HotelId

        CreateMap<ReservaHotel.Modelos.Habitacion, ReservaHotel.Entidades.Habitacion>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Mapear Id a Id
            .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId)); // Mapear HotelId a HotelId

        CreateMap<ReservaHotel.Entidades.Reserva, ReservaHotel.Modelos.Reserva>()
            .ForMember(dest => dest.HabitacionId, opt => opt.MapFrom(src => src.HabitacionId)); // Mapear HabitacionId a HabitacionId

        CreateMap<ReservaHotel.Modelos.Reserva, ReservaHotel.Entidades.Reserva>()
            .ForMember(dest => dest.HabitacionId, opt => opt.MapFrom(src => src.HabitacionId)); // Mapear HabitacionId a HabitacionId
    }
}

//public class MappingProfile : Profile
//{
//    public MappingProfile()
//    {
//        // Mapeo de la entidad Hotel a su correspondiente modelo
//        CreateMap<ReservaHotel.Entidades.Hotel, ReservaHotel.Modelos.Hotel>()
//            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
//            //.ForMember(dest => dest.Habitaciones, opt => opt.MapFrom(src => src.Habitaciones))
//            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
//            .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion));

//        // Mapeo de la entidad Habitacion a su correspondiente modelo
//        CreateMap<ReservaHotel.Entidades.Habitacion, ReservaHotel.Modelos.Habitacion>()
//            //.ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
//            .ForMember(dest => dest.CostoBase, opt => opt.MapFrom(src => src.CostoBase))
//            .ForMember(dest => dest.Impuestos, opt => opt.MapFrom(src => src.Impuestos))
//            .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion))
//            //  .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.HotelId))
//            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
//            .ForMember(dest => dest.CapacidadPersonas, opt => opt.Ignore()); // Ignorando mapeo de CapacidadPersonas;
//        CreateMap<ReservaHotel.Entidades.Habitacion, ReservaHotel.Modelos.Habitacion>();
//        CreateMap<ReservaHotel.Modelos.Habitacion, ReservaHotel.Entidades.Habitacion>();
//        CreateMap<ReservaHotel.Entidades.Hotel, ReservaHotel.Modelos.Hotel>();
//        CreateMap<ReservaHotel.Modelos.Hotel, ReservaHotel.Entidades.Hotel>();

//        // Mapeo entre ReservaHotel.Modelos.Reserva y ReservaHotel.Entidades.Reserva
//        CreateMap<ReservaHotel.Modelos.Reserva, ReservaHotel.Entidades.Reserva>();
//        CreateMap<ReservaHotel.Entidades.Reserva, ReservaHotel.Modelos.Reserva>();

//    }
//}


