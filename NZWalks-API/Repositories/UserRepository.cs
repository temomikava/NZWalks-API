using Npgsql;
using NZWalks_API.Models.Domain;
using System.Data;

namespace NZWalks_API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _connectionString = _configuration.GetConnectionString("MyConnection").Trim();
        }
        public async Task<User> Authenticate(string username, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("authorization", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("__username", username);
                cmd.Parameters.AddWithValue("__password", password);
                connection.Open();
                var reader = await cmd.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    return null;
                }
                var user = new User();
                while (await reader.ReadAsync())
                {
                    user.Id = (Guid)reader["_id"];
                    user.UserName = (string)reader["_username"];
                    user.Password = (string)reader["_password"];
                    user.EmailAddress = (string)reader["_email_address"];
                    user.FirstName = (string)reader["_firstname"];
                    user.LastName = (string)reader["_lastname"];
                }

                var userRoles= await GetUser_rolesByUserId(user.Id);
                if (userRoles.Any())
                {
                    user.Roles = new List<string>();
                    foreach (var userRole in userRoles)
                    {
                        var role=await GetRoleById(userRole.RoleId);
                        if (role!=null)
                        {
                            user.Roles.Add(role.Name);
                        }
                    }
                }

                user.Password = null;
                return user;
            }
        }

        private async Task<IEnumerable<User_Role>> GetUser_rolesByUserId(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                List<User_Role> user_Roles = new List<User_Role>();
                var cmd = new NpgsqlCommand("getuser_roles", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("__userid", id);
                connection.Open();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    User_Role user_Role = new User_Role();
                    user_Role.Id = (Guid)reader["_id"];
                    user_Role.UserId=(Guid)reader["_userid"];
                    user_Role.RoleId=(Guid)reader["_roleid"];
                    user_Roles.Add(user_Role);
                }
                return user_Roles;
            }
        }

        private async Task<Role> GetRoleById(Guid roleId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var cmd = new NpgsqlCommand("getrole", connection) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("__id", roleId);
                connection.Open();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    return new Role()
                    {
                        Id = (Guid)reader["_id"],
                        Name = (string)reader["_name"]
                    };
                }
                return null;
            }

        }
    }
}
