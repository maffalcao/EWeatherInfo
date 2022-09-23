using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherInformationController : ControllerBase
    {
        private readonly ILogger<WeatherInformationController> _logger;

        public WeatherInformationController(ILogger<WeatherInformationController> logger)
        {

        }


    }
}
