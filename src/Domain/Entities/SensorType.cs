using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class SensorType
    {
        public string Name { get; private set; }

        public IList<Measurement> Measurements { get; private set; }
        public SensorType(string name)
        {
            Name = name;
            Measurements = new List<Measurement>();
        }

        public void AddMeasurement(DateTime date, decimal value) => Measurements.Add(new Measurement(date, value));
    }
}