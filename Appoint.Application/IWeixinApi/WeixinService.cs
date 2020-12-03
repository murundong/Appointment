using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.WeixData;
using BaseClasses;

namespace Appoint.Application.IWeixinApi
{
    public class WeixinService : IWeixinService
    {

        

        public W_AUTH_RETURN GetOpenIdByCode(string code)
        {
            W_AUTH_RETURN res = new W_AUTH_RETURN();
            if (!string.IsNullOrWhiteSpace(code))
            {
                string appid = ConstConfig.APPID;
                string appsecret = ConstConfig.APPSECRET;
                string url = string.Format(ConstConfig.auth_code2Session, appid, appsecret, code);

                string content= HttpFunction.Get(url);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    res = JsonSerializer.DeserializeFromString<W_AUTH_RETURN>(content);
                }
            }
            return res;
        }
    }
}
