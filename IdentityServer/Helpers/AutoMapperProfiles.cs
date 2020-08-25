using AutoMapper;
using IdentityServer.Models;
using IdentityServer4.Models;
//using IdentityServer4.EntityFramework.Entities;
using System.Linq;
using IdentityServer.Extensions;

namespace IdentityServer.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<IdentityResource, IdentityResourceDto>().ReverseMap();
            CreateMap<ApiResource, ApiResourceDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap()
            .ForMember(dest => dest.ClientSecrets, opt =>
            {
                opt.MapFrom(src => src.ClientSecrets.Select(p => p.GetShaSecret()));
            })
            .ForMember(dest => dest.AllowedGrantTypes, opt =>
            {
                opt.MapFrom(src => src.AllowedGrantTypes.Select(p => p.ToLower()));
            });
        }
    }
}
