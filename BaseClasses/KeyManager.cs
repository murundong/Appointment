using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    public class KeyManager
    {
        public enum EnumCacheDirections
        {

           ActPTPlayCount
           
        }

     

        public static string GetCacheKey(EnumCacheDirections directions, List<string> list)
        {
            string sideName = "act5211_20171001_";
            switch (directions)
            {
                case EnumCacheDirections.ActPTPlayCount: return sideName + "ActPTPlayCount_";
              

                default: return string.Empty;
            }
        }
    }
}
