using Npgsql;
using NZWalks_API.Models.Domain;
using System.Data;

namespace NZWalks_API.Repositories
{
    public class WalksRepository : ISqlRepository<Walk,Guid>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ISqlRepository<Region,Guid> _regionRepository;
        private readonly ISqlRepository<WalkDifficulty,Guid> _walkDifficultyRepo;

        public WalksRepository(IConfiguration configuration,ISqlRepository<Region,Guid> region, ISqlRepository<WalkDifficulty,Guid> difficultyRepo)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MyConnection").Trim();
            _regionRepository = region;
            _walkDifficultyRepo = difficultyRepo;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    var cmd = new NpgsqlCommand("createwalk", connection) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("_id", walk.Id);
                    cmd.Parameters.AddWithValue("_length", walk.Length);
                    cmd.Parameters.AddWithValue("_name", walk.Name);
                    cmd.Parameters.AddWithValue("_walkdifficultyid", walk.WalkDifficultyId);
                    cmd.Parameters.AddWithValue("_regionid", walk.RegionId);
                    connection.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {

            var walk = GetAsync(id).Result;
            if (walk == null)
            {
                return null;
            }
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("deletewalk", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("_id", id);
                connection.Open();
                await cmd.ExecuteNonQueryAsync();
            }
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            List<Walk> walks = new List<Walk>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("getallwalks", connection) { CommandType = CommandType.StoredProcedure };
                connection.Open();
                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    walks.Add(new Walk()
                    {
                        Id = (Guid)reader["_id"],
                        Length = (double)reader["_length"],
                        Name = (string)reader["_name"],
                        RegionId = (Guid)reader["_regionid"],
                        WalkDifficultyId = (Guid)reader["_walkdifficultyid"],
                        Region = await _regionRepository.GetAsync((Guid)reader["_regionid"]),
                        WalkDifficulty=await _walkDifficultyRepo.GetAsync((Guid)reader["_walkdifficultyid"])
                    });
                }
                return walks;
            }
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("getwalkbyid", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("_id", id);
                connection.Open();
                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.ReadAsync().Result)
                {
                    return new Walk()
                    {
                        Id = (Guid)reader["__id"],
                        Length = (double)reader["_length"],
                        Name = (string)reader["_name"],
                        RegionId = (Guid)reader["_regionid"],
                        WalkDifficultyId = (Guid)reader["_walkdifficultyid"],
                        Region = await _regionRepository.GetAsync((Guid)reader["_regionid"]),
                        WalkDifficulty = await _walkDifficultyRepo.GetAsync((Guid)reader["_walkdifficultyid"])
                    };
                }
                return null;
            }
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = GetAsync(id).Result;
            if (existingWalk==null)
            {
                return null;
            }
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("updatewalk", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("_id",id);
                cmd.Parameters.AddWithValue("_name",walk.Name);
                cmd.Parameters.AddWithValue("_length",walk.Length);
                cmd.Parameters.AddWithValue("_regionid",walk.RegionId);
                cmd.Parameters.AddWithValue("_walkdifficultyid",walk.WalkDifficultyId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            walk.Id = id;
            return walk;
        }
    }
}
