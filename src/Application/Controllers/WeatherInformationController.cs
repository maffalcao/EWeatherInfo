using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherInformationController : ControllerBase
    {
        private readonly ILogger<WeatherInformationController> _logger;
        private readonly IMeasurementService _service;

        public WeatherInformationController(ILogger<WeatherInformationController> logger, IMeasurementService measurementService)
        {
            _logger = logger;
            _service = measurementService;
        }

        public async Task<IList<MeasurementEntity>> GetMeasurements(string deviceId, string sensorType, DateTime date) =>
            await _service.GetMeasurements(deviceId, sensorType, date);




    }
}
