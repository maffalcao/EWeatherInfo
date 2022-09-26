using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class SensorTypeEntity
    {
        public string Name { get; private set; }

        public IList<MeasurementEntity> Measurements { get; private set; }
        public SensorTypeEntity(string name)
        {
            Name = name;
            Measurements = new List<MeasurementEntity>();
        }

        public void AddMeasurement(DateTime date, decimal value) => Measurements.Add(new MeasurementEntity(date, value));
    }
}