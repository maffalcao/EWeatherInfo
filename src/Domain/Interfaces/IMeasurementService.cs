using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMeasurementService
    {
        Task<IList<MeasurementEntity>> Get(string deviceId, string sensorType, DateTime date);
        Task<IList<SensorType>> Get(string deviceId, DateTime date);
    }
}