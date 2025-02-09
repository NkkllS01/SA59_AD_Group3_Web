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
            _connectionString = jObject["ConnectionStrings"]["DefaultConnection"].ToString();
        }

        public List<Species> GetSpeciesByKeyword(string keyword)
        {
            List<Species> species = new List<Species>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.specieId, s.specieName, s.description, s.highlights, c.categoryId, c.categoryName
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
                                    string description = reader.GetString("Description");
                                    string highlights = reader.GetString("Highlights");
                                    int categoryId = reader.GetInt32("CategoryId");
                                    string categoryName = reader.GetString("CategoryName");

                                    Category category = new Category
                                    {
                                        CategoryId = categoryId,
                                        CategoryName = categoryName
                                    };

                                    species.Add(new Species(
                                        specieId, specieName, description, highlights,category
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
        public List<Species> GetSpeciesByCategoryId(int categoryId)
        {
            List<Species> species = new List<Species>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT SpecieId, SpecieName, Description, Highlights, CategoryId
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
                                string description = reader.GetString("Description");
                                string highlights = reader.GetString("Highlights");
                                int catId = reader.GetInt32("CategoryId");

                                species.Add(new Species(specieId, specieName, description, highlights, catId));
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

        public Species GetSpeciesById(int specieId)
        {
            Species species = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.specieId, s.specieName, s.description, s.highlights, s.categoryId, c.categoryName
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

                                species = new Species(
                                    reader.GetInt32("SpecieId"),
                                    reader.GetString("SpecieName"),
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
                Console.WriteLine("Error fetching species by id: " + ex.Message);
            }

            return species;
        }
        public List<Category> GetSpeciesCategory()
        {
            List<Category> categories = new List<Category>();   

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT CategoryId, CategoryName FROM Categories";

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

                    
            