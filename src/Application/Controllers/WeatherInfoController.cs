using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherInfoController : ControllerBase
    {
        private readonly IMeasurementService _service;

        public WeatherInfoController(IMeasurementService measurementService)
        {
            _service = measurementService;
        }

        [HttpGet]
        public async Task<List<SensorTypeEntity>> GetMeasurements([FromQuery] GetMeasurementsParamsDto dto) =>
            await _service.GetMeasurementsAsync(dto.DeviceId, (DateTime)dto.Date, dto.SensorType);

    }
}
