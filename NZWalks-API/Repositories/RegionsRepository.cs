using Npgsql;
using NZWalks_API.Models.Domain;
using System.Data;

namespace NZWalks_API.Repositories
{
    public class RegionsRepository : IRegionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public RegionsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MyConnection").Trim();
        }

        public IEnumerable<Region> GetAll()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                List<Region> regions = new List<Region>();
                try
                {
                    var cmd = new NpgsqlCommand("getallregions", connection) { CommandType = CommandType.StoredProcedure };
                    connection.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        regions.Add(new Region
                        {
                            Id = (Guid)reader["_id"],
                            Area = (double)reader["_area"],
                            Code = (string)reader["_code"],
                            Lat = (double)reader["_lat"],
                            Long = (double)reader["_long"],
                            Name = (string)reader["_name"],
                            Population = (long)reader["_long"]
                        });
                    }

                }
                catch (Exception)
                {

                    throw;
                }
                return regions;
            }
        }
    }
}
