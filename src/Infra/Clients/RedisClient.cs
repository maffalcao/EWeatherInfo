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
        public async Task<T> GetData<T>(string key) where T : class
        {
            return JsonConvert.DeserializeObject<T>(
                await _database.StringGetAsync(key)
            );
        }

        public async void SetData<T>(string key, T content) where T : class
        {
            await _database.StringSetAsync(
                key,
                JsonConvert.SerializeObject(content)
            );
        }
    }
}