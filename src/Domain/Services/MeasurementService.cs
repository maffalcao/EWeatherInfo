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

        public MeasurementService(IMeasurementRepository measurementRepository)
        {
            _measurementRepository = measurementRepository;
        }
        public async Task<IList<MeasurementEntity>> Get(string deviceId, string sensorType, DateTime date)
        {
            return await _measurementRepository.GetDataAsync(deviceId, sensorType, date);
        }

        public async Task<IList<SensorType>> Get(string deviceId, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}