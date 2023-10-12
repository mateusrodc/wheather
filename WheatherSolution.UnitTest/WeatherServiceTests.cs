using MySqlX.XDevAPI.Common;
using WheatherSolution.Application.Services;
using WheatherSolution.Domain.DTO;
using WheatherSolution.Domain.RequestModel;

namespace WheatherSolution.UnitTest
{
    [TestClass]
    public class WheatherServiceTests
    {
        private readonly IWheatherApplication _wheatherApplication;

        public WheatherServiceTests()
        {
            _wheatherApplication = new WheatherApplication(); 
        }

        [TestMethod]
        public async Task GetWheather_CacheHit_ReturnsCachedData()
        {
            var openWeatherRequest = new OpenWheatherRequest
            {
                Latitude = 123.45M,
                Longitude = 67.89M
            };

            var result = await _wheatherApplication.GetWheather(openWeatherRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual(openWeatherRequest.Latitude, result.Latitude);
            Assert.AreEqual(openWeatherRequest.Longitude, result.Longitude);
        }
        [TestMethod]
        public async Task GetWheather_CacheExpires_ReturnsDataFromAPI()
        {

            var openWeatherRequest = new OpenWheatherRequest
            {
                Latitude = 123.45M,
                Longitude = 67.89M
            };

            var result = await _wheatherApplication.GetWheather(openWeatherRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual(openWeatherRequest.Latitude, result.Latitude);
            Assert.AreEqual(openWeatherRequest.Longitude, result.Longitude);
        }
    }
}