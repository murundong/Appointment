using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    public static class MethodExtension
    {
        #region 数据库相关
        public static T ConvertTo<T>(this SqlParameter sqlParameter, T defaultValue = default(T))
        {
            try
            {
                var value = sqlParameter.Value;
                return (T)value;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion
    }
}
