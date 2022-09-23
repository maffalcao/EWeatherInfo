using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class MeasurementService : IMeasurementService
    {
        public IList<Measurement> Get(string deviceId, string sensorType, DateTime date)
        {
            throw new NotImplementedException();
        }

        public IList<SensorType> Get(string deviceId, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}