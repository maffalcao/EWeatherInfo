using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMeasurementService
    {
        Task<List<SensorTypeEntity>> GetMeasurements(string deviceId, DateTime date, string sensorTypeName = null);
    }
}