using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IList<MeasurementEntity>> GetMeasurements(string deviceId, string sensorType, DateTime date)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceId);

            if (device == null)
                throw new Exception("Device not found");
            else if (!device.HasSensorType(sensorType))
                throw new Exception("Device does not have this sensorType");

            return await _measurementRepository.GetMeasurementsAsync(deviceId, sensorType, date);

        }

        public async Task<IList<SensorTypeEntity>> GetMeasurements(string deviceId, DateTime date)
        {
            var device = _deviceRepository.GetByIdAsync(deviceId);

            if (device == null)
                throw new Exception("Device not found");

            return new List<SensorTypeEntity>();


        }
    }
}