using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class DeviceEntity
    {
        public string Id { get; private set; }
        public IList<SensorTypeEntity> SensorTypes { get; private set; }

        public DeviceEntity(string id)
        {
            Id = id;
            SensorTypes = new List<SensorTypeEntity>();
        }

        public void AddSensorType(string sensorTypeName)
        {

            if (!SensorTypes.Any(s => s.Name.Equals(sensorTypeName)))
            {
                SensorTypes.Add(new SensorTypeEntity(sensorTypeName));
            }

        }

        public bool HasSensorType(string sensorTypeName) => SensorTypes.Any(_ => _.Name.Equals(sensorTypeName));

        public bool IsNew() => !SensorTypes.Any();

        public SensorTypeEntity GetSensorType(string sensorTypeName) => SensorTypes.SingleOrDefault(_ => _.Name.Equals(sensorTypeName));

    }
}