using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.ViewData;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework
{
    public class MappingConfigProfile:Profile
    {
        public MappingConfigProfile()
        {
            CreateMap<UserInfo, View_UinfoOutput>();
        }
    }
}
