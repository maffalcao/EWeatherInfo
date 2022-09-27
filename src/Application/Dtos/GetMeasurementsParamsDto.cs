using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class GetMeasurementsParamsDto
    {
        [Required(ErrorMessage = "DeviceId is mandatory")]
        public string DeviceId { get; set; }
        [Required(ErrorMessage = "Date is mandatory")]
        public DateTime? Date { get; set; }
        public string SensorType { get; set; } = null;
    }
}