using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheatherSolution.Domain.DTO;

namespace WheatherSolution.Domain.Interface
{
    public interface IWheatherRepository
    {
        Task<GetWheatherDTO?> GetWheather(string city);
        Task<bool> CreateWheather(CreateWheatherDTO createWheatherDTO);
        IDatabase? GetRedisConfig();
    }
}
