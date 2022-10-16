using Npgsql;
using NZWalks_API.Models.Domain;
using System.Data;

namespace NZWalks_API.Repositories
{
    public class WalkdifficultyRepo : IWalkDifficultyRepo
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public WalkdifficultyRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MyConnection").Trim();
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("createwalkdifficulty", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("_id", walkDifficulty.Id);
                cmd.Parameters.AddWithValue("_name",walkDifficulty.Code);
                connection.Open();
                await cmd.ExecuteNonQueryAsync();
                return walkDifficulty;
            }
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDifficulty=await GetAsync(id);
            if (walkDifficulty==null)
            {
                return null;
            }
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("deletewalkdifficulty", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("_id", id);
                connection.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            List<WalkDifficulty> result = new List<WalkDifficulty>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("getallwalkdifficulty", connection) { CommandType = CommandType.StoredProcedure };
                connection.Open();
                var reader=await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result.Add(new WalkDifficulty()
                    {
                        Id=(Guid)reader["_id"],
                        Code=(string)reader["_name"]
                    });
                }
                return result;
            }
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("getwalkdifficultybyid", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("_id", id);
                connection.Open();
                var reader =await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    return new WalkDifficulty()
                    {
                        Code = (string)reader["_name"],
                        Id = id
                    };
                }
                return null;
            }
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWaldDifficulty =await GetAsync(id);
            if (existingWaldDifficulty==null)
            {
                return null;
            }
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("updatewalkdifficulty", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("_id",id);
                cmd.Parameters.AddWithValue("_name",walkDifficulty.Code);
                connection.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            walkDifficulty.Id = id;
            return walkDifficulty;

        }
    }
}
