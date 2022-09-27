using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class DeviceEntity
    {
        public string Id { get; private set; }
        public List<SensorTypeEntity> SensorTypes { get; private set; }

        public DeviceEntity(string id)
        {
            Id = id;
            SensorTypes = new List<SensorTypeEntity>();
        }

        public void AddSensorType(string sensorTypeName)
        {
            if (!SensorTypes.Any(s => s.Name.Equals(sensorTypeName, StringComparison.OrdinalIgnoreCase)))
                SensorTypes.Add(new SensorTypeEntity(sensorTypeName));
        }

        public bool HasSensorType(string sensorTypeName) =>
            SensorTypes
                .Any(_ => _.Name.Equals(sensorTypeName, StringComparison.OrdinalIgnoreCase));

        public List<SensorTypeEntity> GetSensorTypes(string sensorTypeName) =>
            SensorTypes
                .Where(_ => sensorTypeName == null || _.Name.Equals(sensorTypeName, StringComparison.OrdinalIgnoreCase))
                .ToList();

        public bool IsNew() =>
            !SensorTypes.Any();

    }
}