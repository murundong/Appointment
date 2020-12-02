using Enyim.Caching;
using Enyim.Caching.Configuration;

namespace BaseClasses.MemcachedBase
{
    internal class ClientManager
    {
        private MemcachedClient _client;

        public ClientManager(IMemcachedClientConfiguration configuration)
        {
            this._client = new MemcachedClient(configuration);
        }

        protected MemcachedClient Client
        {
            get { return this._client; }
        }
    }
}
