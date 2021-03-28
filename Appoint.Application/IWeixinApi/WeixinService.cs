using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
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

                string content = HttpFunction.Get(url);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    res = JsonSerializer.DeserializeFromString<W_AUTH_RETURN>(content);
                }
            }
            return res;
        }

        public W_TOKEN_RETURN GetToken()
        {
            W_TOKEN_RETURN res = new W_TOKEN_RETURN();
            string appid = ConstConfig.APPID;
            string secret = ConstConfig.APPSECRET;
            string url = string.Format(ConstConfig.get_accessToken, appid, secret);
            try
            {
                string content = HttpFunction.Get(url);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    res = JsonSerializer.DeserializeFromString<W_TOKEN_RETURN>(content);
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

     

        public W_RETURN SendSubsCribe(W_SUBS_DATA_INPUT input)
        {
            W_RETURN res = new W_RETURN();
            string url = string.Format(ConstConfig.send_subscribe,input.access_token);
            try
            {
                string content = HttpFunction.PostJson(url, JsonSerializer.SerializeToString(input));
                if (!string.IsNullOrWhiteSpace(content))
                {
                    res = JsonSerializer.DeserializeFromString<W_RETURN>(content);
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
