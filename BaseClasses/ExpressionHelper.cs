using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    public static class ExpressionHelper
    {
        //public static void 
        public static DateTime GetNowMonth(this DateTime dt)
        {
            var res= dt.AddHours(-6);
            return new DateTime(res.Year, res.Month,1);
        }
    }
}
