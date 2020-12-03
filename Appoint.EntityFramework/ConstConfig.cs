using BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework
{
    public class ConstConfig
    {
        public static string APPID = ConfigurationHelper.GetAppSetting<string>("APPID");
        public static string APPSECRET = ConfigurationHelper.GetAppSetting<string>("APPSECRET");

        public const string auth_code2Session = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
        
    }
}
