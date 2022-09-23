using System.Threading.Tasks;

namespace Infra.Interfaces
{
    public interface IRedisClient
    {
        Task<T> GetData<T>(string key) where T : class;
        void SetData<T>(string key, T content) where T : class;
    }
}