using System.Collections.Generic;

namespace Domain.Entities
{
    public class SensorTypeEntity
    {
        public string Name { get; private set; }

        public List<MeasurementEntity> Measurements { get; private set; }
        public SensorTypeEntity(string name)
        {
            Name = name;
            Measurements = new List<MeasurementEntity>();
        }

        public void AddMeasurements(List<MeasurementEntity> measurements) =>
            Measurements.AddRange(measurements);
    }
}