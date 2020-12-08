using Billing.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
  public  class YYSignHelper
    {
        public string CreateParamsSign(ParamsBase data, string key, out string str)
        {
            str = string.Empty;
            if (data == null) return string.Empty;
            var t = data.GetType();
            var dic = new Dictionary<string, string>();
            foreach (var p in t.GetProperties())
            {
                var objAttrs = p.GetCustomAttributes(typeof(EntityMappingAttribute), true);
                if (objAttrs.Length > 0)
                {
                    var attr = objAttrs[0] as EntityMappingAttribute;
                    if (attr != null)
                    {
                        if (attr.Type != Enum_BusinessType.Sign) continue;
                    }
                }

                if (p.Name.ToLower().Equals("sign") || p.Name.ToLower().Equals("key")) continue;

                var _t = p.PropertyType;
                if ((!_t.IsPrimitive) && _t != typeof(string) && _t != typeof(DateTime)) continue;  //过滤非基元类型且不为string,DateTime类型的字段

                var val = p.GetValue(data, null);

                if (_t == typeof(DateTime))
                {
                    val = Utils.TimeToShuttle(Convert.ToDateTime(val));                             //如果字段为DateTime类型,将值转换为long(时间戳)
                }

                dic.Add(p.Name, val == null ? string.Empty : val.ToString());
            }
            var sortParamsStr = CreateDataString(dic);
            sortParamsStr += string.Format("&key={0}", key);
            str = sortParamsStr;
            return Utils.MD5(sortParamsStr);
        }

        private string CreateDataString(IDictionary<string, string> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            var dataParams = new StringBuilder();
            foreach (var kv in vDic)
            {
                var key = kv.Key;
                var value = kv.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    dataParams.Append(key).Append("=").Append(string.Format("{0}", value)).Append("&");
                }
            }
            return dataParams.ToString().Substring(0, dataParams.ToString().Length - 1);
        }

    }

    [Serializable]
    public class ParamsBase
    {
        public int Bid { get; set; }

        public string Sign { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class EntityMappingAttribute : Attribute
    {
        public Enum_BusinessType Type { get; set; }
    }


    public enum Enum_BusinessType
    {
        [Description("None")]
        None = 0,

        [Description("登录凭据校验")]
        ST = 1,

        [Description("加密串校验")]
        Sign = 2,

        [Description("客户端IP校验")]
        IP = 4,

        [Description("域名校验")]
        Domain = 8
    }

}
