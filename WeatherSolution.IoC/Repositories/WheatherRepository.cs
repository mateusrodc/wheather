using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WheatherSolution.Domain.DTO;
using WheatherSolution.Domain.Interface;

namespace WeatherSolution.IoC.Repositories
{
    public class WheatherRepository: IWheatherRepository
    {
        private readonly SqlContext _context;
        private readonly RedisConfig _redisConfig;

        public WheatherRepository(SqlContext context, RedisConfig redisConfig)
        {
            _context = context;
            _redisConfig = redisConfig;
        }

        public async Task<bool> CreateWheather(CreateWheatherDTO createWheatherDTO)
        {
            using(var connection = _context.CreateConnection()) 
            {
                connection.Open();
                using (var transact = connection.BeginTransaction())
                {
                    var rowsAffected = await transact.Connection.ExecuteAsync(
                            sql: @"INSERT INTO dev.wheather (latitude, longitude, temperature, minimumTemperature, maximumTemperature, createdAt, updatedAt, city)
                                    VALUES (@latitude, @longitude, @temperature, @minimumTemperature, @maximumTemperature, @createdAt, @updatedAt, @city);",
                            param: new
                            {
                                latitude = createWheatherDTO.Latitude,
                                longitude = createWheatherDTO.Longitude,
                                temperature = createWheatherDTO.Temperature,
                                minimumTemperature = createWheatherDTO.MinimumTemperature,
                                maximumTemperature = createWheatherDTO.MaximumTemperature,
                                createdAt = DateTime.UtcNow,
                                updatedAt = DateTime.UtcNow,
                                city = createWheatherDTO.City
                            },
                            transaction: transact);

                    transact.Commit();
                }
                connection.Close();
            }
            return true;
        }

        public async Task<GetWheatherDTO?> GetWheather(string city)
        {
            using(var connection = _context.CreateConnection()) 
            {
                connection.Open();

                var wheather = await connection.QueryFirstOrDefaultAsync<GetWheatherDTO>(
                    sql: @"SELECT
                                Latitude as latitude,
                                Longitude as longitude,
                                Temperature as temperature,
                                MinimumTemperature as minimumTemperature,
                                MaximumTemperature as maximumTemperature,
                                City as city
                            FROM dev.wheather
                            WHERE city = @City
                            ORDER BY createdAt DESC
                            LIMIT 1",
                    param: new
                    {
                        City = city
                    });

                return wheather;
            }
        }

        StackExchange.Redis.IDatabase? IWheatherRepository.GetRedisConfig()
        {
            return _redisConfig.RedisConfiguration();
        }
    }
}
