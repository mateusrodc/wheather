using Microsoft.AspNetCore.Mvc;
using WheatherSolution.Application.Services;
using WheatherSolution.Domain.RequestModel;

namespace WeatherApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWheatherApplication _wheatherApplication;
        public WeatherController(IWheatherApplication wheatherApplication) 
        {
            _wheatherApplication = wheatherApplication;
        }

        [HttpGet]
        [Route("by-city")]
        public async Task<IActionResult> GetWheather([FromQuery] OpenWheatherRequest openWheatherRequest)
        {
            var result = await _wheatherApplication.GetWheather(openWheatherRequest);
            return Ok(result);
        }
    }
}
