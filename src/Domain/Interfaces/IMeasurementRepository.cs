using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<List<MeasurementEntity>> GetMeasurementsAsync(string deviceId, string sensorType, DateTime date);
    }
}