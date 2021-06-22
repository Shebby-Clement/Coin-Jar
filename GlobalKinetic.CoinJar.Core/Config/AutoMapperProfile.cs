using AutoMapper;
using GlobalKinetic.CoinJar.Core.Dtos;
using GlobalKinetic.CoinJar.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalKinetic.CoinJar.Core.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CoinDto, Coin>();
            CreateMap<Coin, CoinDto>();
        }
    }
}