using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMeasurementService
    {
        public IList<Measurement> Get(string deviceId, string sensorType, DateTime date);
        public IList<SensorType> Get(string deviceId, DateTime date);
    }
}