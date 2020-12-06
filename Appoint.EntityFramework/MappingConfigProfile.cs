﻿using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.ViewData;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework
{
    public class MappingConfigProfile : Profile
    {
        public MappingConfigProfile()
        {
            CreateMap<UserInfos, View_UinfoOutput>()
                .ForMember(des => des.birthday, opt => opt.MapFrom(src => ConvertTimeToString(src.birthday)));
                //.ForMember(des => des.name, opt => opt.MapFrom(src => src.show_name));
            CreateMap<Banners, View_BannerOutput>();
            CreateMap<Doors, View_DoorsOutput>();
        }

        Func<DateTime?, string> ConvertTimeToString = delegate (DateTime? s)
        {
            if (s.HasValue) return Convert.ToDateTime(s).ToString("yyyy-MM-dd");
            return null;
        };
    }
}
