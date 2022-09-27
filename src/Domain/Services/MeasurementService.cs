using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossCutting.Exceptions;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepository _measurementRepository;
        private readonly IDeviceRepository _deviceRepository;

        public MeasurementService(IMeasurementRepository measurementRepository, IDeviceRepository deviceRepository)
        {
            _measurementRepository = measurementRepository;
            _deviceRepository = deviceRepository;
        }
        public async Task<List<SensorTypeEntity>> GetMeasurements(string deviceId, DateTime date, string sensorTypeName = null)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null)
                throw new ValidationException("Device not found");

            else if (sensorTypeName != null && !device.HasSensorType(sensorTypeName))
                throw new ValidationException("Device does not have this sensorType");

            else if (date.Date > DateTime.Today)
                throw new ValidationException("Date cannot be in the future");

            var sensorTypes = device.GetSensorTypes(sensorTypeName);

            // TODO: refactor to enable parallel iteration 
            foreach (var sensorType in sensorTypes)
            {
                var measurements = await _measurementRepository.GetMeasurementsAsync(deviceId, sensorType.Name, date);

                if (measurements != null)
                    sensorType.AddMeasurements(measurements);
            }

            return sensorTypes;

        }
    }
}