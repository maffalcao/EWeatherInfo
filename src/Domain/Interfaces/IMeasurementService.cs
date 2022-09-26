using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMeasurementService
    {
        Task<IList<MeasurementEntity>> GetMeasurements(string deviceId, string sensorType, DateTime date);
        Task<IList<SensorTypeEntity>> GetMeasurements(string deviceId, DateTime date);
    }
}