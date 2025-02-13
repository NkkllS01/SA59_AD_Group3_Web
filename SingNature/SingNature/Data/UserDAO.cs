using MySql.Data.MySqlClient;
using authorization.Models;
using Newtonsoft.Json.Linq;

namespace authorization.Data
{
    public class UserDao
    {
        private readonly string _connectionString; 

        public UserDao() {
            var json = File.ReadAllText("appsettings.json");
            var jObject = JObject.Parse(json);
            _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        }

        public User? GetUserByUsername(string username)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM User WHERE UserName = @Username";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32("UserId"),
                                UserName = reader.GetString("UserName"),
                                Password = reader.GetString("Password"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Mobile = reader.IsDBNull(reader.GetOrdinal("Mobile")) ? null : reader.GetString("Mobile"),
                                Warning = reader.GetBoolean("Warning"),
                                Newsletter = reader.GetBoolean("Newsletter")
                            };
                        }
                    }
                }
            }
            return null;
        }


        public User? GetUserById(int userId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM User WHERE UserId = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32("UserId"),
                                UserName = reader.GetString("UserName"),
                                Password = reader.GetString("Password"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Mobile = reader.IsDBNull(reader.GetOrdinal("Mobile")) ? null : reader.GetString("Mobile"),
                                Warning = reader.GetBoolean("Warning"),
                                Newsletter = reader.GetBoolean("Newsletter")
                            };
                        }
                    }
                }
            }
            return null;
        }


        public void CreateUser(User user)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO User (UserName, Password, Email, Mobile, Warning, Newsletter) " +
                             "VALUES (@UserName, @Password, @Email, @Mobile, @Warning, @Newsletter)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mobile", user.Mobile ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Warning", user.Warning);
                    cmd.Parameters.AddWithValue("@Newsletter", user.Newsletter);


                    cmd.ExecuteNonQuery();
                }
            }
        }

 
        public void UpdateUser(User user)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE User SET UserName = @UserName, Email = @Email, Mobile = @Mobile, " +
                             "Warning = @Warning, Newsletter = @Newsletter " +
                             "WHERE UserId = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mobile", user.Mobile ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Warning", user.Warning);
                    cmd.Parameters.AddWithValue("@Newsletter", user.Newsletter);
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void UpdateSubscription(int userId, bool warning, bool newsletter)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE User SET Warning = @Warning, Newsletter = @Newsletter WHERE UserId = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Warning", warning);
                    cmd.Parameters.AddWithValue("@Newsletter", newsletter);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
