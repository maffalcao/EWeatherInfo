using System.Threading.Tasks;

namespace Infra.Interfaces
{
    public interface IRedisClient
    {
        Task<T> GetDataAsync<T>(string key) where T : class;
        Task SetDataAsync<T>(string key, T content) where T : class;
    }
}