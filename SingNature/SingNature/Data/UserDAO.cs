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
            _connectionString = jObject["ConnectionStrings"]["DefaultConnection"].ToString();
        }

        public User? GetUserByUsername(string username)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM user WHERE Username = @Username";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("UserId"),
                                Username = reader.GetString("Username"),
                                Password = reader.GetString("Password"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Mobile")) ? null : reader.GetString("Mobile"),
                                SubscribeWarning = reader.GetBoolean("Warning"),
                                SubscribeNewsletter = reader.GetBoolean("Newsletter")
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
                string sql = "SELECT * FROM user WHERE Id = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("UserId"),
                                Username = reader.GetString("Username"),
                                Password = reader.GetString("Password"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Mobile")) ? null : reader.GetString("Mobile"),
                                SubscribeWarning = reader.GetBoolean("Warning"),
                                SubscribeNewsletter = reader.GetBoolean("Newsletter")
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
                string sql = "INSERT INTO user (Username, Password, Email, Mobile, Warning, Newsletter) " +
                             "VALUES (@Username, @Password, @Email, @Phone, @SubscribeWarning, @SubscribeNewsletter)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubscribeWarning", user.SubscribeWarning);
                    cmd.Parameters.AddWithValue("@SubscribeNewsletter", user.SubscribeNewsletter);


                    cmd.ExecuteNonQuery();
                }
            }
        }

 
        public void UpdateUser(User user)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE user SET Username = @Username, Email = @Email, Mobile = @Phone, " +
                             "Warning = @SubscribeWarning, Newsletter = @SubscribeNewsletter " +
                             "WHERE UserId = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SubscribeWarning", user.SubscribeWarning);
                    cmd.Parameters.AddWithValue("@SubscribeNewsletter", user.SubscribeNewsletter);
                    cmd.Parameters.AddWithValue("@UserId", user.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void UpdateSubscription(int userId, bool subscribeWarning, bool subscribeNewsletter)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE user SET Warning = @SubscribeWarning, Newsletter = @SubscribeNewsletter WHERE UserId = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SubscribeWarning", subscribeWarning);
                    cmd.Parameters.AddWithValue("@SubscribeNewsletter", subscribeNewsletter);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
