using System;
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
    public class MeasurementRepository : BaseRepository<MeasurementEntity>, IMeasurementRepository
    {
        private string _deviceId { get; set; }
        private string _sensorType { get; set; }
        private DateTime _date { get; set; }

        public MeasurementRepository(IRedisClient redis, IIO io, IOptions<AppSettings> settings) : base(redis, io, settings) { }
        public async Task<List<MeasurementEntity>> GetMeasurementsAsync(string deviceId, string sensorType, DateTime date)
        {
            _deviceId = deviceId;
            _sensorType = sensorType;
            _date = date;

            List<MeasurementEntity> measurements = null;

            var dateIsToday = date.Date == DateTime.Today;

            /*
                if the search date is not today, you can fetch the data from redis or local file. 
                Otherwise, it will always download a new file for research.
            */
            if (!dateIsToday)
            {
                measurements = await _redis.GetDataAsync<List<MeasurementEntity>>(RedisKey);

                if (measurements == null)
                    measurements = GetDataFromFile();
            }

            if (measurements == null)
                measurements = await GetDataFromUrlAsync();


            if (measurements == null)
                measurements = await GetDataFromHistoryAsync(date);

            if (!dateIsToday)
                await _redis.SetDataAsync<List<MeasurementEntity>>(RedisKey, measurements, _settings.MeasurementsMinutesToExpireInRedis);

            return measurements;
        }


        protected override List<MeasurementEntity> GetDataFromFile()
        {
            List<MeasurementEntity> measurements = null;

            if (_io.FileExists(CsvFilePath))
            {
                measurements = new List<MeasurementEntity>();

                using (StreamReader sr = new StreamReader(CsvFilePath))
                {
                    string currentLine;

                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        var token = currentLine.Split(";");

                        if (token.Count() != NUMBER_OF_COLUMNS_IN_CSV)
                            ThrowFormatException(CsvFilePath, currentLine);

                        var isValidDate = DateTime.TryParse(token[0], out var measurementDate);
                        var isValidDecimal = Decimal.TryParse(token[1], out var measurementValue);

                        if (!isValidDate || !isValidDecimal)
                            ThrowFormatException(CsvFilePath, currentLine);

                        measurements.Add(
                            new MeasurementEntity(measurementDate, measurementValue)
                        );
                    }
                }
            }

            return measurements;
        }

        protected override async Task<List<MeasurementEntity>> GetDataFromUrlAsync()
        {
            List<MeasurementEntity> measurements = null;

            if (await _io.IsUrlAvailable(CsvUrlPath))
            {
                _io.DownloadFile(CsvUrlPath, DestionationFolder);
                measurements = GetDataFromFile();
            }

            return measurements;
        }

        private async Task<List<MeasurementEntity>> GetDataFromHistoryAsync(DateTime date)
        {
            List<MeasurementEntity> measurements = null;

            /*

            If historical.zip file already exist locally, it has already been unzipped into the base folder. 
            In this scenario, according to the second step above (get data from file) that file is not contained in .zip 
            when we arrive here.

            But it is possible that the file being searched is recent (last 10 days, recently compressed). If so, the historical.zip file is 
            downloaded again and unzipped. Otherwise, nothing is done and an empty result is returned.

            */

            var historyFilePathExists = _io.FileExists(HistoryFilePath);
            var historyUrlExists = await _io.IsUrlAvailable(HistoryUrlPath);
            var dontHasPastMaxDaysToAFileBecompressed = (DateTime.Now - date).Days < _settings.MaxDaysFromNowToAFileBeCompressed;

            if (
                (
                    !historyFilePathExists ||
                    dontHasPastMaxDaysToAFileBecompressed
                ) &&
                historyUrlExists)
            {
                // if we are here, it means it will take a long time to complete download & unpack tasks
                _io.DownloadFile(HistoryUrlPath, DestionationFolder);
                _io.UnzipFile(HistoryFilePath, DestionationFolder);

                measurements = GetDataFromFile();

            }

            return measurements;
        }

        private string DestionationFolder =>
            Path.Combine(_folderBase, _deviceId, _sensorType);

        private string CsvFilePath =>
            Path.Combine(_folderBase, _deviceId, _sensorType, $"{_date.ToString("yyyy-MM-dd")}.csv");

        private string HistoryFilePath =>
            Path.Combine(_folderBase, _deviceId, _sensorType, "historical.zip");

        private string CsvUrlPath =>
            @$"{_urlBase}/{_deviceId}/{_sensorType}/{_date.ToString("yyyy-MM-dd")}.csv";
        private string HistoryUrlPath =>
            @$"{_urlBase}/{_deviceId}/{_sensorType}/historical.zip";

        private string RedisKey =>
            $"weatherinfo-{_deviceId}-{_sensorType}-{_date.ToString("yyyy-MM-dd")}";



    }
}