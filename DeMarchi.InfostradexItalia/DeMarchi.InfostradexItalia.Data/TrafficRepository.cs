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
    public class TrafficRepository : ITrafficRepository
    {
        private readonly string _connectionString;
        public TrafficRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Traffic> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Traffic>();
            }
        }

        public Traffic GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Get<Traffic>(id);
            }
        }
        public IEnumerable<Traffic> GetState()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Traffic>(@"SELECT TOP(10) * FROM [dbo].[TrafficTable] ORDER BY Id DESC");
            }
        }

        public IEnumerable<Traffic> GetConteggio()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Traffic>(@"
                SELECT
                        [Id_incrocio]
                        ,[Id_semaforo]
                        ,[Id_strada]
                        ,[Fascia_oraria]
                        ,[Data]
                        ,[Tipologia_veicolo]
                        ,SUM(Conteggio) AS Conteggio
                FROM [dbo].[TrafficTable]
                WHERE Data = CAST(GETDATE() AS date)
                GROUP BY Tipologia_veicolo, Fascia_oraria, Id_incrocio, 
                    Id_semaforo, Id_strada, Data
                ORDER BY Fascia_oraria ASC
                ");
            }
        }

        public void Insert(Traffic value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Insert(value);
            }
        }

        public void Update(Traffic value)
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
                connection.Delete(new Traffic
                {
                    Id = id
                });
            }
        }

        public IEnumerable<Traffic> GetAutomobili()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Traffic>(@"
                SELECT
                        [Id_incrocio]
                        ,[Id_semaforo]
                        ,[Id_strada]
                        ,[Fascia_oraria]
                        ,[Data]
                        ,[Tipologia_veicolo]
                        ,SUM(Conteggio) AS Conteggio
                FROM [dbo].[TrafficTable]
                WHERE Data = CAST(GETDATE() AS date) and Tipologia_veicolo = 'Automobile'
                GROUP BY Tipologia_veicolo, Fascia_oraria, Id_incrocio, 
                    Id_semaforo, Id_strada, Data
                ORDER BY Fascia_oraria ASC
                ");
            }
        }

        public IEnumerable<Traffic> GetCamion()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Traffic>(@"
                SELECT
                        [Id_incrocio]
                        ,[Id_semaforo]
                        ,[Id_strada]
                        ,[Fascia_oraria]
                        ,[Data]
                        ,[Tipologia_veicolo]
                        ,SUM(Conteggio) AS Conteggio
                FROM [dbo].[TrafficTable]
                WHERE Data = CAST(GETDATE() AS date) and Tipologia_veicolo = 'Camion'
                GROUP BY Tipologia_veicolo, Fascia_oraria, Id_incrocio, 
                    Id_semaforo, Id_strada, Data
                ORDER BY Fascia_oraria ASC
                ");
            }
        }

        public IEnumerable<Traffic> GetMotociclo()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Traffic>(@"
                SELECT
                        [Id_incrocio]
                        ,[Id_semaforo]
                        ,[Id_strada]
                        ,[Fascia_oraria]
                        ,[Data]
                        ,[Tipologia_veicolo]
                        ,SUM(Conteggio) AS Conteggio
                FROM [dbo].[TrafficTable]
                WHERE Data = CAST(GETDATE() AS date) and Tipologia_veicolo = 'Motociclo'
                GROUP BY Tipologia_veicolo, Fascia_oraria, Id_incrocio, 
                    Id_semaforo, Id_strada, Data
                ORDER BY Fascia_oraria ASC
                ");
            }
        }
    }
}
