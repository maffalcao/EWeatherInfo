using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Interfaces;

namespace Infra.Repositories
{
    public class MeasurementRepository : BaseRepository<MeasurementEntity>, IMeasurementRepository
    {
        private readonly int MAX_DAYS_FROM_NOW_TO_BE_COMPRESSED = 10;
        //private string _dataPath;
        //private string _dataFilePath;

        private string _deviceId { get; set; }
        private string _sensorType { get; set; }
        private DateTime _date { get; set; }

        public MeasurementRepository(IRedisClient redis, IIO io) : base(redis, io)
        {

        }
        public async Task<IList<MeasurementEntity>> GetMeasurementsAsync(string deviceId, string sensorType, DateTime date)
        {
            _deviceId = deviceId;
            _sensorType = sensorType;
            _date = date;

            //_dataPath = $"{deviceId}/{sensorType}";
            //_dataFilePath = $"{_dataPath}/{date.ToString("yyyy-MM-dd")}";

            var key = $"{_deviceId}-{_sensorType}-{date}";

            var measurements = await _redis.GetDataAsync<IList<MeasurementEntity>>(key);

            try
            {

                if (measurements == null)
                    measurements = GetDataFromFile();
                if (measurements == null)
                    measurements = await GetDataFromUrlAsync();
                //if (measurements == null)
                //   measurements = await GetDataFromHistoryAsync(date);

                //await _redis.SetDataAsync<IList<MeasurementEntity>>(key, measurements);
            }
            catch (Exception ex)
            {
                var bla = 1;
            }


            return measurements;
        }


        protected override IList<MeasurementEntity> GetDataFromFile()
        {
            List<MeasurementEntity> measurements = null;
            var filePath = GetCsvFilePath();

            if (_io.FileExists(filePath))
            {
                measurements = new List<MeasurementEntity>();

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string currentLine;

                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        var token = currentLine.Split(";");
                        measurements.Add(
                            new MeasurementEntity(DateTime.Parse(token[0]), Decimal.Parse(token[1]))
                        );
                    }
                }
            }

            return measurements;
        }

        protected override async Task<IList<MeasurementEntity>> GetDataFromUrlAsync()
        {
            List<MeasurementEntity> measurements = null;
            var url = GetCsvUrlPath();

            if (await _io.IsUrlAvailable(url))
            {
                _io.DownloadFile(url, GetDestionationFolder());
                measurements = GetDataFromFile().ToList();
            }

            return measurements;
        }

        private async Task<IList<MeasurementEntity>> GetDataFromHistoryAsync(DateTime date)
        {
            List<MeasurementEntity> measurements = null;

            if (
                (
                    !_io.FileExists(GetHistoryFilePath()) ||
                    (DateTime.Now - date).Days < MAX_DAYS_FROM_NOW_TO_BE_COMPRESSED
                ) &&
                await _io.IsUrlAvailable(GetHistoryUrlPath()))
            {

                _io.DownloadFile(GetHistoryUrlPath(), GetDestionationFolder());
                measurements = GetDataFromFile().ToList();

            }

            return measurements;
        }

        public string GetDestionationFolder() =>
            @$"{_folderBase}\{_deviceId}\{_sensorType}\";

        public string GetCsvFilePath() =>
            @$"{_folderBase}\{_deviceId}\{_sensorType}\{_date.ToString("yyyy-MM-dd")}.csv";

        public string GetHistoryFilePath() =>
            @$"{_folderBase}\{_deviceId}\{_sensorType}\historical.zip";

        public string GetCsvUrlPath() =>
            @$"{_urlBase}/{_deviceId}/{_sensorType}/{_date.ToString("yyyy-MM-dd")}.csv";
        public string GetHistoryUrlPath() =>
            @$"{_urlBase}/{_deviceId}/{_sensorType}/historical.zip";



    }
}