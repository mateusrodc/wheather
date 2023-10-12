using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using WheatherSolution.Domain.Models;

namespace WeatherSolution.IoC
{
    public class SqlContext : DbContext
    {
        public SqlContext()
        {

        }
        public SqlContext(DbContextOptions<SqlContext> options) : base(options) { }

        public DbSet<Weather> Wheather { get; set; }


        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .Build();

        public IDbConnection CreateConnection()
        {
            string connectionString = configuration?.GetSection("Dev").Value ?? string.Empty;
            return new MySqlConnection(connectionString);
        }
    }
}