using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appoint.EntityFramework.WeixData
{
    public class W_SUBS_DATA_INPUT
    {
        public string access_token { get; set; }
        public string touser { get; set; }
        public string template_id { get; set; }
        public string page { get; set; }
        public object data { get; set; }
        public string miniprogram_state { get; set; } = "developer";
        public string lang { get; set; } = "zh_CN";
    }
}
