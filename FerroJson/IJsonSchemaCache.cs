using System.Runtime.Caching;

namespace FerroJson
{
    public interface IJsonSchemaCacheProvider
    {
        IJsonSchema Get(string key);
        IJsonSchema Remove(string key);
        void Set(string key, IJsonSchema schema);
    }

    public class DefaultJsonSchemaCacheProvider : IJsonSchemaCacheProvider
    {
        private readonly ObjectCache _cache;
        private readonly CacheItemPolicy _policy;

        public DefaultJsonSchemaCacheProvider()
        {
            _cache = MemoryCache.Default;
            _policy = new CacheItemPolicy();
        }

        public IJsonSchema Get(string key)
        {
            if (!_cache.Contains(key))
                return null;

            return (IJsonSchema)_cache[key];
        }

        public IJsonSchema Remove(string key)
        {
            if (!_cache.Contains(key))
                return null;

            return (IJsonSchema)_cache.Remove(key);
        }

        public void Set(string key, IJsonSchema schema)
        {
            _cache.Set(key, schema, _policy);
        }
    }
}