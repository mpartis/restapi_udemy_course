using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile // tou AutoMapper
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap(); // basic mapping, yparxei kai manual
            // CreateMap<VillaDTO, Villa>();

            CreateMap<Villa, VillaCreateDTO>().ReverseMap(); // gia na mhn ta grafw 2 fores
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        }
    }
}
