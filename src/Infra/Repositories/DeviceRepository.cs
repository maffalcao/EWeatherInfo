using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting.Settings;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Interfaces;
using Microsoft.Extensions.Options;

namespace Infra.Repositories
{
    public class DeviceRepository : BaseRepository<DeviceEntity>, IDeviceRepository
    {

        private const string REDIS_KEY = "weatherinfo-devices";
        private const string METADATA_FILE_NAME = "metadata.csv";
        public DeviceRepository(IRedisClient redis, IIO io, IOptions<AppSettings> settings) : base(redis, io, settings) { }

        public async Task<DeviceEntity> GetByIdAsync(string deviceId)
        {
            var devices = await _redis.GetDataAsync<List<DeviceEntity>>(REDIS_KEY);

            if (devices == null)
            {
                devices = await GetDataFromUrlAsync();

                await _redis.SetDataAsync<List<DeviceEntity>>(REDIS_KEY, devices, _settings.DevicesMinutesToExpireInRedis);
            }

            return devices.SingleOrDefault(_ => _.Id == deviceId);
        }

        protected override List<DeviceEntity> GetDataFromFile()
        {
            List<DeviceEntity> devices = null;
            var filePath = Path.Combine(_folderBase, METADATA_FILE_NAME);

            if (_io.FileExists(filePath))
            {
                devices = new List<DeviceEntity>();

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string currentLine;

                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        var token = currentLine.Split(";");

                        if (token.Count() != NUMBER_OF_COLUMNS_IN_CSV)
                            ThrowFormatException(filePath, currentLine);

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

        protected override async Task<List<DeviceEntity>> GetDataFromUrlAsync()
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