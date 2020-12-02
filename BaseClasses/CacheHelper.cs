using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BaseClasses.MemcachedBase;

namespace BaseClasses
{
    public class CacheHelper
    {
        public enum CacheType
        {
            WebCache = 1,
            MemCache = 2
        }

        private static CacheType cacheType = (CacheType)Enum.Parse(typeof(CacheType), ConfigurationHelper.GetAppSetting<string>("CacheType", "MemCache"));

        private static IEntities _cache;
        static CacheHelper()
        {
            switch (cacheType)
            {
                case CacheType.WebCache:
                    _cache = new WebCache();
                    break;
                case CacheType.MemCache:
                    _cache = Entities.Cache;
                    break;
            }
        }

        public static bool Add(string key, object value)
        {
            return _cache.Add(key, value);
        }

        public static bool Add(string key, object value, long lNumofMilliSeconds)
        {
            return _cache.Add(key, value, lNumofMilliSeconds);
        }

        public static bool Add(string key, object value, TimeSpan timeSpan)
        {
            return _cache.Add(key, value, timeSpan);
        }

        public static T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public static object Get(string key)
        {
            return _cache.Get(key);
        }

        public static Dictionary<K, T> Gets<K, T>(List<string> keys, List<K> outKey)
        {
            return _cache.Gets<K, T>(keys, outKey);
        }

        public static void RemoveAll()
        {
            _cache.RemoveAll();
        }

        public static bool Remove(string key)
        {
            return _cache.Remove(key);
        }

        public static long Increment(string key, long amount)
        {
            return _cache.Increment(key, amount);
        }

        public static long Decrement(string key, long amount)
        {
            return _cache.Decrement(key, amount);
        }
    }
}
