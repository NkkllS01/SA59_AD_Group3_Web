using MySql.Data.MySqlClient;
using authorization.Models;

namespace authorization.Data
{
    public class UserDao
    {
        private readonly string _connectionString = "server=localhost;uid=root;pwd=Gzj20011027;database=AdProject";

  
        public User? GetUserByUsername(string username)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM users WHERE Username = @Username";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("Id"),
                                Username = reader.GetString("Username"),
                                Password = reader.GetString("Password"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
                                SubscribeWarning = reader.GetBoolean("SubscribeWarning"),
                                SubscribeNewsletter = reader.GetBoolean("SubscribeNewsletter")
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
                string sql = "SELECT * FROM users WHERE Id = @UserId";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("Id"),
                                Username = reader.GetString("Username"),
                                Password = reader.GetString("Password"),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
                                SubscribeWarning = reader.GetBoolean("SubscribeWarning"),
                                SubscribeNewsletter = reader.GetBoolean("SubscribeNewsletter")
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
                string sql = "INSERT INTO users (Username, Password, Email, Phone, SubscribeWarning, SubscribeNewsletter) " +
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
                string sql = "UPDATE users SET Username = @Username, Email = @Email, Phone = @Phone, " +
                             "SubscribeWarning = @SubscribeWarning, SubscribeNewsletter = @SubscribeNewsletter " +
                             "WHERE Id = @UserId";

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
                string sql = "UPDATE users SET SubscribeWarning = @SubscribeWarning, SubscribeNewsletter = @SubscribeNewsletter WHERE Id = @UserId";

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
