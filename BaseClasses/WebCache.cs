using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BaseClasses.MemcachedBase;

namespace BaseClasses
{
    public class WebCache : IEntities
    {
        public bool Add(string key, object value)
        {
            var lNumofMilliSeconds = 60000;
            if (string.IsNullOrEmpty(key) || value == null || lNumofMilliSeconds <= 0)
                return false;
            if (HttpRuntime.Cache[key] == null)
                HttpRuntime.Cache.Add(key, value, null, DateTime.Now.AddMilliseconds(lNumofMilliSeconds), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            else HttpRuntime.Cache[key] = value;
            return true;
        }

        public bool Add(string key, object value, long lNumofMilliSeconds)
        {
            if (string.IsNullOrEmpty(key) || value == null || lNumofMilliSeconds <= 0)
                return false;

            if (HttpRuntime.Cache[key] == null)
                HttpRuntime.Cache.Add(key, value, null, DateTime.Now.AddMilliseconds(lNumofMilliSeconds), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            else HttpRuntime.Cache[key] = value;
            return true;
        }

        public bool Add(string key, object value, TimeSpan timeSpan)
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return false;

            if (HttpRuntime.Cache[key] == null)
                HttpRuntime.Cache.Add(key, value, null, DateTime.Now.Add(timeSpan), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            else HttpRuntime.Cache[key] = value;
            return true;
        }
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            if (HttpRuntime.Cache[key] == null) return false;
            HttpRuntime.Cache.Remove(key);
            return true;
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(T);

            return (T)HttpRuntime.Cache.Get(key);
        }

        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return HttpRuntime.Cache.Get(key);
        }

        public Dictionary<K, T> Gets<K, T>(List<string> keys, List<K> outKey)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            var caches = HttpRuntime.Cache.GetEnumerator();
            while (caches.MoveNext())
            {
                HttpRuntime.Cache.Remove(caches.Key.ToString());
            }
        }

        public long Increment(string key, long amount)
        {
            throw new NotImplementedException();
        }

        public long Decrement(string key, long amount)
        {
            throw new NotImplementedException();
        }
    }
}
