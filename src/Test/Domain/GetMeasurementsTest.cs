using System;
using System.Threading.Tasks;
using CrossCutting.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Moq;
using Xunit;

namespace Domain
{
    public class GetMeasurementsTest
    {
        private IMeasurementService service;
        private Mock<IMeasurementRepository> _measurementRepositoryMock;
        private Mock<IDeviceRepository> _deviceRepositoryMock;

        public GetMeasurementsTest()
        {
            _measurementRepositoryMock = new Mock<IMeasurementRepository>();
            _deviceRepositoryMock = new Mock<IDeviceRepository>();
        }

        [Fact]
        public async Task Should_Fail_When_DeviceId_Does_Not_Exist()
        {
            _deviceRepositoryMock.Setup(_ => _.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((DeviceEntity)null);
            service = new MeasurementService(_measurementRepositoryMock.Object, _deviceRepositoryMock.Object);

            try
            {
                var measurements = await service.GetMeasurementsAsync(It.IsAny<string>(), It.IsAny<DateTime>());
                Assert.True(false);
            }
            catch (ValidationException ex)
            {
                Assert.Equal("Device not found", ex.Message);
            }
        }

        [Theory]
        [InlineData("deviceId1", "sensorTypeFromDevice1", "sensorTypeGiven1")]
        [InlineData("deviceId2", "sensorTypeFromDevice2", "sensorTypeGiven2")]

        public async Task Should_Fail_When_SensorType_Does_Not_Exist_For_A_Existent_DeviceId(string deviceId, string deviceIdSensorTypeName, string givenSensorTypeName)
        {
            var device = new DeviceEntity(deviceId);
            device.AddSensorType(deviceIdSensorTypeName);

            _deviceRepositoryMock.Setup(_ => _.GetByIdAsync(deviceId)).ReturnsAsync(device);
            service = new MeasurementService(_measurementRepositoryMock.Object, _deviceRepositoryMock.Object);

            try
            {
                var measurements = await service.GetMeasurementsAsync(deviceId, It.IsAny<DateTime>(), givenSensorTypeName);
                Assert.True(false);
            }
            catch (ValidationException ex)
            {
                Assert.Equal("Device does not have this sensorType", ex.Message);
            }
        }

        [Theory]
        [InlineData("deviceId1", "sensorTypeFromDevice1")]

        public async Task Should_Fail_When_Date_Is_In_Future(string deviceId, string deviceIdSensorTypeName)
        {
            var device = new DeviceEntity(deviceId);
            device.AddSensorType(deviceIdSensorTypeName);

            _deviceRepositoryMock.Setup(_ => _.GetByIdAsync(deviceId)).ReturnsAsync(device);
            service = new MeasurementService(_measurementRepositoryMock.Object, _deviceRepositoryMock.Object);

            try
            {

                var date = DateTime.Now.AddDays(1);
                var measurements = await service.GetMeasurementsAsync(deviceId, date, deviceIdSensorTypeName);
                Assert.True(false);
            }
            catch (ValidationException ex)
            {
                Assert.Equal("Date cannot be in the future", ex.Message);
            }
        }

        [Theory, MemberData(nameof(MeasurementsParamsData))]

        public async Task Should_Be_Succeed_If_All_Parameters_Are_Valid(string deviceId, string deviceIdSensorTypeName, DateTime date)
        {
            var device = new DeviceEntity(deviceId);
            device.AddSensorType(deviceIdSensorTypeName);

            _deviceRepositoryMock.Setup(_ => _.GetByIdAsync(deviceId)).ReturnsAsync(device);

            service = new MeasurementService(_measurementRepositoryMock.Object, _deviceRepositoryMock.Object);

            var measurements = await service.GetMeasurementsAsync(deviceId, date, deviceIdSensorTypeName);
            Assert.True(true);
        }

        public static readonly object[][] MeasurementsParamsData =
        {
            new object[] { "deviceId1", "sensorTypeFromDevice1", DateTime.Now.AddYears(-1)},
            new object[] { "deviceId2", "sensorTypeFromDevice2", DateTime.Now},

        };




    }
}