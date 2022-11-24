using AutoMapper;
using Practica.TiendasAPI.Entities;
using Practica.TiendasAPI.Models;

namespace Practica.TiendasAPI.Profiles
{
    public class TiendaProfile : Profile
    {
        public TiendaProfile()
        {
            CreateMap<Tienda, TiendaDto>();
            CreateMap<Tienda, TiendaSinProductosDto>();
        }
    }
}
