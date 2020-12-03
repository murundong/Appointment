using Appoint.EntityFramework.WeixData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.Application.IWeixinApi
{
    public interface IWeixinService:IApplicationService
    {
        W_AUTH_RETURN GetOpenIdByCode(string code);
    }
}
