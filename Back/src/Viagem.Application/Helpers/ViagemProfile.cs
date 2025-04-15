using System;
using AutoMapper;
using Viagem.Application.Dtos;
using Viagem.Domain;

namespace Viagem.API.Helpers
{
    public class ViagemProfile : Profile
    {
        public ViagemProfile()
        {
            CreateMap<Rota, RotaDto>().ReverseMap();
        }
    }
}