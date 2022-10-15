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

        public async Task<Region> AddRegion(Region region)
        {
            region.Id = Guid.NewGuid();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new NpgsqlCommand("createregion", connection) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("_id", region.Id);
                    cmd.Parameters.AddWithValue("_code", region.Code);
                    cmd.Parameters.AddWithValue("_name", region.Name);
                    cmd.Parameters.AddWithValue("_area", region.Area);
                    cmd.Parameters.AddWithValue("_lat", region.Lat);
                    cmd.Parameters.AddWithValue("_long", region.Long);
                    cmd.Parameters.AddWithValue("_population", region.Population);
                    connection.Open();
                    await cmd.ExecuteNonQueryAsync();

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region=GetAsync(id).Result;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new NpgsqlCommand("deleteregion", connection) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("_id", id);
                    connection.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            List<Region> regions = new List<Region>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new NpgsqlCommand("getallregions", connection) { CommandType = CommandType.StoredProcedure };
                    connection.Open();
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (reader.ReadAsync().Result)
                    {
                        regions.Add(new Region
                        {
                            Id = (Guid)reader["_id"],
                            Area = (double)(reader["_area"]),
                            Code = (string)reader["_code"],
                            Lat = (double)(reader["_lat"]),
                            Long = (double)(reader["_long"]),
                            Name = (string)reader["_name"],
                            Population = (long)(reader["_population"])
                        });
                    }
                    return regions;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public async Task<Region> GetAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new NpgsqlCommand("getregionbyid", connection) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("_id", id);
                    connection.Open();
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (reader.ReadAsync().Result)
                    {
                        return new Region
                        {
                            Id = (Guid)reader["__id"],
                            Area = (double)(reader["_area"]),
                            Code = (string)reader["_code"],
                            Lat = (double)(reader["_lat"]),
                            Long = (double)(reader["_long"]),
                            Name = (string)reader["_name"],
                            Population = (long)(reader["_population"])
                        };
                    }
                    return null;
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }
        }
    }
}
