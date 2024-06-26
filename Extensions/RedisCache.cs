
using System;
using System.Collections.Generic;
using SqlSugar;

namespace Wee_XYYY.Extensions
{
    /// <summary>
    /// Redis操作方法
    /// 说明：添加键值对、添加json，以及修改删除等
    /// </summary>
    public class RedisCache : ICacheService
    {
        public void Add<V>(string key, V value)
        {
            RedisServer.Cache.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            RedisServer.Cache.Set(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<V>(string key)
        {
            return RedisServer.Cache.Exists(key);
        }

        public V Get<V>(string key)
        {
            return RedisServer.Cache.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return RedisServer.Cache.Keys("Cache:SqlSugarDataCache.*");
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (ContainsKey<V>(cacheKey))
            {
                return Get<V>(cacheKey);
            }
            else
            {
                var result = create();
                Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<V>(string key)
        {
            RedisServer.Cache.Del(key);
        }
    }
}

