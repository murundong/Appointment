using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses.MemcachedBase
{
    internal class EntitiesDefault : ClientManager, IEntities
    {
        public EntitiesDefault(IMemcachedClientConfiguration configuration)
            : base(configuration)
        {

        }

        public bool Add(string key, object value)
        {
            return this.Client.Store(StoreMode.Set, key, value);
        }

        public bool Add(string key, object value, long lNumofMilliSeconds)
        {
            return this.Client.Store(StoreMode.Set, key, value, DateTime.Now.AddMilliseconds(lNumofMilliSeconds));
        }

        public bool Add(string key, object value, TimeSpan timeSpan)
        {
            try
            { 
            return this.Client.Store(StoreMode.Set, key, value, timeSpan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Get<T>(string key)
        {
            return this.Client.Get<T>(key);
        }

        public IDictionary<string, object> Get(IEnumerable<string> keys)
        {
            return this.Client.Get(keys);
        }

        public Dictionary<K, T> Gets<K, T>(List<string> keys, List<K> outKey)
        {
            var data = this.Client.Get(keys);
            Dictionary<K, T> retData = new Dictionary<K, T>();
            for (int i = 0; i < keys.Count; i++)
            {
                object value = null;
                data.TryGetValue(keys[i], out value);
                if (!retData.ContainsKey(outKey[i]))
                {
                    retData.Add(outKey[i], (T)value);
                }
            }
            return retData;
        }

        public object Get(string key)
        {
            try
            { 
            return this.Client.Get(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveAll()
        {
            this.Client.FlushAll();
        }

        public bool Remove(string key)
        {
            return this.Client.Remove(key);
        }

        public long Increment(string key, long amount)
        {
            return (long)this.Client.Increment(key, default(ulong), (ulong)amount);
        }

        public long Decrement(string key, long amount)
        {
            return (long)this.Client.Decrement(key, default(ulong), (ulong)amount);
        }
    }

    public interface IEntities
    {
        bool Add(string key, object value);
        bool Add(string key, object value, long lNumofMilliSeconds);
        bool Add(string key, object value, TimeSpan timeSpan);
        T Get<T>(string key);
        object Get(string key);
        Dictionary<K, T> Gets<K, T>(List<string> keys, List<K> outKey);
        void RemoveAll();
        bool Remove(string key);
        long Increment(string key, long amount);
        long Decrement(string key, long amount);
    }

    public class Entities
    {
        static IEntities _cache;
        static object _cacheLockObj = new object();
        public static IEntities Cache
        {
            get
            {
                if (_cache == null)
                {
                    lock (_cacheLockObj)
                    {
                        if (_cache == null)
                        {
                            _cache = new EntitiesDefault(Config.Configurations);
                        }
                    }
                }
                return _cache;
            }
        }
    }
}
