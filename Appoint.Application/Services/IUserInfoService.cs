using Appoint.EntityFramework.ViewData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.Services
{
    public interface IUserInfoService:IApplicationService
    {
        View_UinfoOutput GetUserInfo(string openid);
    }
}
