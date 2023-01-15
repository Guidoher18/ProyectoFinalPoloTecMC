using Dapper;
using Microsoft.Extensions.Configuration;
using Modelos.Models;
using Newtonsoft.Json;
using Repositorio.Interfaces;
using System.Data.SqlClient;

namespace Repositorio
{
    public class BingoRepository : IBingoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public BingoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString").Value ?? "";
        }

        public void Add(Datos entity)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                DateTime date = DateTime.Now;

                var id = sqlConnection.ExecuteScalar($"INSERT INTO HistorialBolillero (FechaHora, Bolillas) OUTPUT INSERTED.IdBolillero VALUES (@FechaHora, @Bolillas);", new { 
                    FechaHora = date,
                    Bolillas = JsonConvert.SerializeObject(entity.Bolillero)
                });

                sqlConnection.Execute($"INSERT INTO HistorialCartones (IdBolillero, FechaHora, Carton1, Carton2, Carton3, Carton4) VALUES (@IdBolillero, @FechaHora, @Carton1, @Carton2, @Carton3, @Carton4);", new { 
                    IdBolillero = id,
                    FechaHora = date,
                    Carton1 = entity.Carton1,
                    Carton2 = entity.Carton2,   
                    Carton3 = entity.Carton3,
                    Carton4 = entity.Carton4
                });
            }
        }
    }
}