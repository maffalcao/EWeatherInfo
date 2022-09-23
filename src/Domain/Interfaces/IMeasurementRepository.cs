using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<IList<MeasurementEntity>> GetDataAsync(string deviceId, string sensorType, DateTime date);
        IList<MeasurementEntity> GetDataFromHistoricalZip(DateTime date);
        IList<MeasurementEntity> GetDataFromCsvFile();
        IList<MeasurementEntity> GetDataFromUrlCsvFile();

    }
}