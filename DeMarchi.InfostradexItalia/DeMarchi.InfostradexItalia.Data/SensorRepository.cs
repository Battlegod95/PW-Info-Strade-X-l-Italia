using Dapper;
using Dapper.Contrib.Extensions;
using DeMarchi.InfostradexItalia.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeMarchi.InfostradexItalia.Data
{
    public class SensorRepository : ISensorRepository
    {
        private readonly string _connectionString;
        public SensorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Sensor> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Sensor>();
            }
        }

        public Sensor GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Get<Sensor>(id);
            }
        }
        public IEnumerable<Sensor> GetLastSensor()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Sensor>(@"SELECT TOP(1) * FROM [dbo].[SensorTable] ORDER BY Id DESC");
            }
        }

        public void Insert(Sensor value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Insert(value);
            }
        }

        public void Update(Sensor value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update(value);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Delete(new Sensor
                {
                    Id = id
                });
            }
        }
    }
}
