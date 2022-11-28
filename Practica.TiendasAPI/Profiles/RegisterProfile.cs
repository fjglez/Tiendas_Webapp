using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Practica.TiendasAPI.Entities;
using Practica.TiendasAPI.Models;

namespace Practica.TiendasAPI.Profiles
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterViewModel, IdentityUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email)); ;
        }   
    }
}
