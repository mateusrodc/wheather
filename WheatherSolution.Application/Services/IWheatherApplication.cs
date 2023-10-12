using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using StackExchange.Redis;
using System.Configuration;
using System.Text.RegularExpressions;
using WheatherSolution.Domain.DTO;
using WheatherSolution.Domain.Interface;
using WheatherSolution.Domain.RequestModel;
using WheatherSolution.Domain.ResponseModel;

namespace WheatherSolution.Application.Services
{
    public interface IWheatherApplication
    {
        Task<GetWheatherDTO?> GetWheather(OpenWheatherRequest openWheatherRequest);
    }
    public class WheatherApplication : IWheatherApplication
    {
        private readonly IWheatherRepository _wheatherRepository;
        private readonly IConfiguration _configuration;

        public WheatherApplication(IWheatherRepository wheatherRepository, IConfiguration configuration)
        {
            _wheatherRepository = wheatherRepository;
            _configuration = configuration;
        }

        public WheatherApplication()
        {
        }

        public async Task<GetWheatherDTO?> GetWheather(OpenWheatherRequest openWheatherRequest)
        {
            var cache = _wheatherRepository.GetRedisConfig();

            var cachedData = cache.StringGet($"{openWheatherRequest.Latitude}-{openWheatherRequest.Longitude}");
            if (cachedData.HasValue)
            {
                // Se estiver no cache, retorne os dados
                return JsonConvert.DeserializeObject<GetWheatherDTO?>(cachedData);
            }
            else
            {
                var requestUrlApi = MakeRequestUrl(openWheatherRequest.Latitude, openWheatherRequest.Longitude);
                var responseApi = await ConsumeOpenWeather(requestUrlApi);

                var createWheatherDTO = MakeCreateRequest(responseApi);

                await _wheatherRepository.CreateWheather(createWheatherDTO);

                cache.StringSet($"{openWheatherRequest.Latitude}-{openWheatherRequest.Longitude}", JsonConvert.SerializeObject(createWheatherDTO), TimeSpan.FromMinutes(20));

                return await _wheatherRepository.GetWheather(createWheatherDTO.City);
            }
        }
        #region PrivateMethods
        private string MakeRequestUrl(decimal? latitude, decimal? longitude)
        {
            var openWheather = _configuration.GetSection("OpenWheather").Value;
            var ApiKey = _configuration.GetSection("ApiKey").Value;
            return $"{openWheather}lat={latitude}&lon={longitude}&appid={ApiKey}";
        }
        private async Task<GetWheatherDTO?> ConsumeOpenWeather(string? request)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(request);

                if(response.IsSuccessStatusCode) 
                {
                    string data = await response.Content.ReadAsStringAsync();

                    var wheatherResponse = JsonConvert.DeserializeObject<OpenWheatherResponse>(data);

                    GetWheatherDTO wheatherDTO = new GetWheatherDTO()
                    {
                        Latitude = wheatherResponse.Coordinates.Latitude,
                        Longitude = wheatherResponse.Coordinates.Longitude,
                        City = wheatherResponse.City,
                        Temperature = wheatherResponse.Main.Temperature,
                        MaximumTemperature = wheatherResponse.Main.MaximumTemperature,
                        MinimumTemperature = wheatherResponse.Main.MinimumTemperature,
                    };

                    return wheatherDTO;
                }

                return new GetWheatherDTO();
            }
        }
        private CreateWheatherDTO MakeCreateRequest(GetWheatherDTO? wheatherDTO)
        {
            var city = RemoveCaracteresEspeciais(wheatherDTO.City);
            return new CreateWheatherDTO()
            {
                Latitude = wheatherDTO.Latitude,
                Longitude = wheatherDTO.Longitude,
                City = city,
                Temperature = wheatherDTO.Temperature,
                MinimumTemperature = wheatherDTO.MinimumTemperature,
                MaximumTemperature = wheatherDTO.MaximumTemperature,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
        }
        private static string RemoveCaracteresEspeciais(string city)
        {
            string resultado = Regex.Replace(city, @"\p{IsCombiningDiacriticalMarks}+", string.Empty);

            return resultado;
        }
        #endregion
    }
}