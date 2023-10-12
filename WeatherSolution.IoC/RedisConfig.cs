using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherSolution.IoC
{
    public class RedisConfig
    {
        private readonly IConfiguration _configuration;
        public RedisConfig(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public IDatabase? RedisConfiguration()
        {
            var redisSettings = _configuration.GetSection("RedisSettings");
            var redis = ConnectionMultiplexer.Connect($"{redisSettings["Server"]}:{redisSettings["Port"]}");

            return redis.GetDatabase();
        }
    }
}
