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

        public static string template_cancel= ConfigurationHelper.GetAppSetting<string>("template_cancel");
        public static string template_notice = ConfigurationHelper.GetAppSetting<string>("template_notice");
        public static string template_queue = ConfigurationHelper.GetAppSetting<string>("template_queue");

        public const string auth_code2Session = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
        public const string get_accessToken = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        public const string send_subscribe = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}";
    }
}
