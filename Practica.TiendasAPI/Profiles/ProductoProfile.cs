using AutoMapper;
using Practica.TiendasAPI.Entities;
using Practica.TiendasAPI.Models;

namespace Practica.TiendasAPI.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDto>();
            CreateMap<ProductoCreationDto, Producto>();
            CreateMap<ProductoUpdateDto, Producto>();
            CreateMap<Producto, ProductoUpdateDto>();
        }
    }
}
