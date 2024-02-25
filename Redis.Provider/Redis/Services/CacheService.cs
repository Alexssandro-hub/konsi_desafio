using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using Redis.Provider.Redis.Interfaces;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Redis.Provider.Redis.Services
{
    public class CacheService : ICacheService
    {
        private IDatabase _db;
        public CacheService()
        {
            ConfigureRedis();
        }
        private void ConfigureRedis()
        {
            _db = ConnectionHelper.Connection.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            return !string.IsNullOrEmpty(value) ? JsonConvert.DeserializeObject<T>(value) : default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }

        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);

            return _isKeyExist ? _db.KeyDelete(key) : _isKeyExist;
        }
    }

}
