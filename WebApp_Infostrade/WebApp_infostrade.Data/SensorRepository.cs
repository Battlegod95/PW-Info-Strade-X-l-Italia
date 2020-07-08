using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WebApp_Infostrade.Models;

namespace WebApp_infostrade.Data
{
    public class SensorRepository : ISensorRepository
    {
        private readonly string _connectionstring;
        public SensorRepository(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Sensor> GetAll()
        {
            using(var connection = new SqlConnection(_connectionstring))
            {
                return connection.GetAll<Sensor>();
            }
        }

        public Sensor GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionstring))
            {
                return connection.Get<Sensor>(id);
            }
        }

        public void Insert(Sensor value)
        {
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Insert(value);
            }
        }

        public void Update(Sensor value)
        {
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Update(value);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionstring))
            {
                connection.Delete(new Sensor
                {
                    Id = id
                });
            }
        }
    }
}
