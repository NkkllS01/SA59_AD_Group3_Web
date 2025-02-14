using SingNature.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace SingNature.Data
{
    public class CategoryDAO
    {
        private readonly string _connectionString;

        public CategoryDAO()
        {
            var json = File.ReadAllText("appsettings.json");
            var jObject = JObject.Parse(json);
            _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT CategoryId, CategoryName, ImageUrl FROM Category";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                CategoryId = reader.GetInt32("CategoryId"),
                                CategoryName = reader.GetString("CategoryName"),
                                ImageUrl = reader.GetString("ImageUrl")
                            });
                        }
                    }
                }
            }
            return categories;
        }
    }
}
