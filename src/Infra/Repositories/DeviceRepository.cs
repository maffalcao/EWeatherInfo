using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Interfaces;

namespace Infra.Repositories
{
    public class DeviceRepository : BaseRepository<DeviceEntity>, IDeviceRepository
    {
        public DeviceRepository(IRedisClient redis, IIO io) : base(redis, io)
        {

        }

        public async Task<DeviceEntity> GetByIdAsync(string deviceId)
        {
            var devices = await GetDataFromUrlAsync();
            return devices.SingleOrDefault(_ => _.Id == deviceId);
        }

        protected override IList<DeviceEntity> GetDataFromFile()
        {
            List<DeviceEntity> devices = null;
            var filePath = $"{_folderBase}/metadata.csv";

            if (_io.FileExists(filePath))
            {
                devices = new List<DeviceEntity>();

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string currentLine;

                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        var token = currentLine.Split(";");
                        var deviceId = token[0];
                        var sensorType = token[1];

                        var device = devices.SingleOrDefault(_ => _.Id == deviceId) ?? new DeviceEntity(deviceId);

                        if (device.IsNew())
                            devices.Add(device);

                        device.AddSensorType(sensorType);
                    }
                }
            }

            return devices;
        }

        protected override async Task<IList<DeviceEntity>> GetDataFromUrlAsync()
        {
            List<DeviceEntity> devices = null;
            var url = $"{_urlBase}/metadata.csv";

            if (await _io.IsUrlAvailable(url))
            {

                _io.DownloadFile(url, _folderBase);
                devices = GetDataFromFile().ToList();
            }

            return devices;
        }
    }
}