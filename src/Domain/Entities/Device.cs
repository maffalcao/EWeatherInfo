using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Device
    {
        public string Id { get; private set; }
        public IList<SensorType> SensorTypes { get; private set; }

        public Device(string id)
        {
            Id = id;
            SensorTypes = new List<SensorType>();
        }

        public void AddSensorType(string sensorTypeName)
        {

            if (!SensorTypes.Any(s => s.Name.Equals(sensorTypeName)))
            {
                SensorTypes.Add(new SensorType(sensorTypeName));
            }

        }

        public bool HasSensorType(string sensorTypeName) => SensorTypes.Any(_ => _.Name.Equals(sensorTypeName));

        public SensorType GetSensorType(string sensorTypeName) => SensorTypes.SingleOrDefault(_ => _.Name.Equals(sensorTypeName));

    }
}