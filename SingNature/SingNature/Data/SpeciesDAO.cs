using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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
                    SELECT specieId, specieName, description, highlights
                    FROM Species
                    WHERE LOWER(specieName) LIKE LOWER(@keyword)
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

                                    species.Add(new Species(
                                        specieId, specieName, description, highlights
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

    }
}