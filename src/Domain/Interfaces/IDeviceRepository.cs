using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDeviceRepository
    {
        Task<DeviceEntity> GetByIdAsync(string deviceId);
    }
}