using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SingNature.Data;

namespace SingNature.Data
{
     public class SpeciesDAO
    {
        private readonly string _connectionString;

        public SpeciesDAO()
        {
            var json = File.ReadAllText("appsettings.json");
            var jObject = JObject.Parse(json);
            _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        }

        public List<Specie> GetSpeciesByKeyword(string keyword)
        {
            List<Specie> species = new List<Specie>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.specieId, s.specieName, s.imageUrl, s.description, s.highlights, c.categoryId, c.categoryName
                    FROM Specie s
                    JOIN Category c ON s.categoryId = c.categoryId
                    WHERE LOWER(s.specieName) LIKE LOWER(@keyword)
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("SpecieId")))
                                {
                                    int specieId = reader.GetInt32("SpecieId");
                                    string specieName = reader.GetString("SpecieName");
                                    string imageUrl = reader.GetString("ImageUrl");
                                    string description = reader.GetString("Description");
                                    string highlights = reader.GetString("Highlights");
                                    int categoryId = reader.GetInt32("CategoryId");
                                    string categoryName = reader.GetString("CategoryName");

                                    Category category = new Category
                                    {
                                        CategoryId = categoryId,
                                        CategoryName = categoryName
                                    };

                                    species.Add(new Specie(
                                        specieId, specieName, imageUrl, description, highlights,category
                                    ));
                                } 
                                else 
                                {
                                    Console.WriteLine("Species not found!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching species: " + ex.Message);
            }

            return species;
        }
        public List<Specie> GetSpeciesByCategoryId(int categoryId)
        {
            List<Specie> species = new List<Specie>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT SpecieId, SpecieName, ImageUrl, Description, Highlights, CategoryId
                    FROM Specie
                    WHERE CategoryId = @categoryId";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                            if (!reader.IsDBNull(reader.GetOrdinal("SpecieId")))
                            {
                                int specieId = reader.GetInt32("SpecieId");
                                string specieName = reader.GetString("SpecieName");
                                string imageUrl = reader.GetString("ImageUrl");
                                string description = reader.GetString("Description");
                                string highlights = reader.GetString("Highlights");
                                int catId = reader.GetInt32("CategoryId");

                                species.Add(new Specie(specieId, specieName, imageUrl, description, highlights, catId));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching species by categoryId: " + ex.Message);
        }

        return species;
        }

        public Specie GetSpeciesById(int specieId)
        {
            Specie specie = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.specieId, s.specieName, s.imageUrl, s.description, s.highlights, s.categoryId, c.categoryName
                    FROM Specie s
                    INNER JOIN Category c ON s.categoryId = c.categoryId
                    WHERE s.specieId = @specieId
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@specieId", specieId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int categoryId = reader.GetInt32("CategoryId");
                                string categoryName = reader.GetString("CategoryName");

                                Category category = new Category
                                {
                                    CategoryId = categoryId,
                                    CategoryName = categoryName
                                };

                                specie = new Specie(
                                    reader.GetInt32("SpecieId"),
                                    reader.GetString("SpecieName"),
                                    reader.GetString("ImageUrl"),
                                    reader.GetString("Description"),
                                    reader.GetString("Highlights"),
                                    category
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching specie by id: " + ex.Message);
            }

            return specie;
        }
        public List<Category> GetSpeciesCategory()
        {
            List<Category> categories = new List<Category>();   

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT CategoryId, CategoryName FROM Category";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new Category
                                {
                                    CategoryId = reader.GetInt32("CategoryId"),
                                    CategoryName = reader.GetString("CategoryName")
                                 });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine("Error fetching categories: " + ex.Message);
            }

            return categories;
        }

    }
}

                    
            
                    
            