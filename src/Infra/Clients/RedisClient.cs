using System;
using System.Threading.Tasks;
using Infra.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infra.Clients
{
    public class RedisClient : IRedisClient
    {
        private readonly IDatabase _database;

        public RedisClient(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<T> GetDataAsync<T>(string key) where T : class
        {
            var value = await _database.StringGetAsync(key);
            return JsonConvert.DeserializeObject<T>(
                value.ToString() ?? string.Empty
            );
        }

        public async Task SetDataAsync<T>(string key, T content, int? minutesToExpire = null) where T : class
        {
            var timeSpam = minutesToExpire == null ?
                (TimeSpan?)null :
                TimeSpan.FromMinutes((int)minutesToExpire);

            content = content ?? Activator.CreateInstance<T>();

            await _database.StringSetAsync(
                key,
                JsonConvert.SerializeObject(content),
                timeSpam
            );
        }
    }
}