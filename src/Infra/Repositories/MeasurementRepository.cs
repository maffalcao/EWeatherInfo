using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infra.Interfaces;

namespace Infra.Repositories
{
    public class MeasurementRepository : BaseRepository<MeasurementEntity>
    {
        private readonly int MAX_DAYS_FROM_NOW_TO_BE_COMPRESSED = 10;
        private string _dataPath;
        private string _dataFilePath;

        public MeasurementRepository(IRedisClient redis, IIO io) : base(redis, io)
        {

        }
        public async Task<IList<MeasurementEntity>> GetDataAsync(string deviceId, string sensorType, DateTime date)
        {

            _dataPath = $"{deviceId}/{sensorType}";
            _dataFilePath = $"{_dataPath}/{date}";

            var measurements =
                        (await _redis.GetDataAsync<IList<MeasurementEntity>>(_dataFilePath)) ??
                        GetDataFromCsvFile() ??
                        GetDataFromUrlCsvFile() ??
                        GetDataFromHistoricalZip(date);

            await _redis.SetDataAsync<IList<MeasurementEntity>>(_dataPath, measurements);

            return measurements;
        }

        private IList<MeasurementEntity> GetDataFromHistoricalZip(DateTime date)
        {
            List<MeasurementEntity> measurements = null;

            var historicalPath = $"{_dataPath}/historical.zip";
            var historicalFilePath = $"{_folderBase}/{historicalPath}";
            var historicalUrl = $"{_urlBase}/{historicalPath}";

            if (
                (
                    !_io.FileExists(historicalFilePath) ||
                    (DateTime.Now - date).Days < MAX_DAYS_FROM_NOW_TO_BE_COMPRESSED
                ) &&
                _io.IsUrlAvailable(historicalUrl))
            {

                measurements = new List<MeasurementEntity>();

                _io.DownloadFile(historicalUrl, $"{_folderBase}/{_dataPath}");
                measurements = GetDataFromCsvFile().ToList();

            }

            return measurements;
        }

        public override IList<MeasurementEntity> GetDataFromCsvFile()
        {
            List<MeasurementEntity> measurements = null;
            var filePath = $"{_folderBase}/{_dataFilePath}.csv";

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

        public override IList<MeasurementEntity> GetDataFromUrlCsvFile()
        {
            List<MeasurementEntity> measurements = null;
            var url = $"{_urlBase}/{_dataFilePath}.csv";

            if (_io.IsUrlAvailable(url))
            {
                measurements = new List<MeasurementEntity>();

                var csvFile = $"{_folderBase}/{_dataFilePath}.csv";
                var destinationFolder = $"{_folderBase}/{_dataPath}";

                _io.DownloadFile(url, destinationFolder);
                measurements = GetDataFromCsvFile().ToList();

            }

            return measurements;
        }
    }
}