using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SingNature.Data
{
     public class SightingsDAO
    {
        private readonly string _connectionString;

        public SightingsDAO()
        {
            var json = File.ReadAllText("appsettings.json");
            var jObject = JObject.Parse(json);
            _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        }

        public List<Sighting> GetAllSightings()
        {
            List<Sighting> sightings = new List<Sighting>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.sightingId, s.userId, u.userName, s.date, s.specieId, sp.specieName, s.details, s.imageUrl, s.latitude, s.longitude
                    FROM Sighting s
                    JOIN Specie sp ON s.specieId = sp.specieId
                    JOIN User u ON s.userId = u.userId
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("SightingId")))
                            {
                                int sightingId = reader.GetInt32("SightingId");
                                int userId = reader.GetInt32("UserId");
                                string userName = reader.GetString("UserName");
                                DateTime date = reader.GetDateTime("Date");
                                int specieId = reader.GetInt32("SpecieId");
                                string specieName = reader.GetString("SpecieName");
                                string details = reader.IsDBNull(reader.GetOrdinal("Details")) ? "" : reader.GetString("Details");
                                string imageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl");
                                decimal latitude = reader.GetDecimal("Latitude");
                                decimal longitude = reader.GetDecimal("Longitude");

                                sightings.Add(new Sighting(
                                    sightingId, userId, userName, date, specieId, specieName, details ?? "", imageUrl ?? "", latitude, longitude
                                ));
                            } 
                            else 
                            {
                                Console.WriteLine("SightingId is NULL or not found!");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching sightings: " + ex.Message);
            }

            return sightings;
        }

        public Sighting? GetSightingById(int id)
        {
            Sighting? sighting = null;
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.sightingId, s.userId, u.userName, s.date, s.specieId, sp.specieName, s.details, s.imageUrl, s.latitude, s.longitude
                    FROM Sighting s
                    JOIN Specie sp ON s.specieId = sp.specieId
                    JOIN User u ON s.userId = u.userId
                    WHERE s.SightingId = @id
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("SightingId")))
                                {
                                    int sightingId = reader.GetInt32("SightingId");
                                    int userId = reader.GetInt32("UserId");
                                    string userName = reader.GetString("UserName");
                                    DateTime date = reader.GetDateTime("Date");
                                    int specieId = reader.GetInt32("SpecieId");
                                    string specieName = reader.GetString("SpecieName");
                                    string details = reader.IsDBNull(reader.GetOrdinal("Details")) ? "" : reader.GetString("Details");
                                    string imageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl");
                                    decimal latitude = reader.GetDecimal("Latitude");
                                    decimal longitude = reader.GetDecimal("Longitude");

                                    sighting = new Sighting(
                                        sightingId, userId, userName, date, specieId, specieName, details, imageUrl, latitude, longitude
                                    );
                                }
                                else
                                {
                                    Console.WriteLine("SightingId is NULL or not found!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching sighting: " + ex.Message);
            }

            return sighting;
        }

        public List<Sighting> GetSightingsByKeyword(string keyword)
        {
            List<Sighting> sightings = new List<Sighting>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.sightingId, s.userId, u.userName, s.date, s.specieId, sp.specieName, s.details, s.imageUrl, s.latitude, s.longitude
                    FROM Sighting s
                    JOIN Specie sp ON s.specieId = sp.specieId
                    JOIN User u ON s.userId = u.userId
                    WHERE LOWER(sp.specieName) LIKE LOWER(@keyword)
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal("SightingId")))
                                {
                                    int sightingId = reader.GetInt32("SightingId");
                                    int userId = reader.GetInt32("UserId");
                                    string userName = reader.GetString("UserName");
                                    DateTime date = reader.GetDateTime("Date");
                                    int specieId = reader.GetInt32("SpecieId");
                                    string specieName = reader.GetString("SpecieName");
                                    string details = reader.IsDBNull(reader.GetOrdinal("Details")) ? "" : reader.GetString("Details");
                                    string imageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl");
                                    decimal latitude = reader.GetDecimal("Latitude");
                                    decimal longitude = reader.GetDecimal("Longitude");

                                    sightings.Add(new Sighting(
                                        sightingId, userId, userName, date, specieId, specieName, details ?? "", imageUrl ?? "", latitude, longitude
                                    ));
                                } 
                                else 
                                {
                                    Console.WriteLine("Sightings not found!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching sightings: " + ex.Message);
            }

            return sightings;
        }
        
        public Sighting? CreateSighting(Sighting? sighting)
        {
            if (sighting.SpecieId <= 0)
            {
                throw new ArgumentException("Invalid SpecieId provided.");
            }
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    INSERT INTO Sighting (UserId, Date, SpecieId, Details, ImageUrl, Latitude, Longitude) 
                    VALUES (@UserId, @Date, @SpecieId, @Details, @ImageUrl, @Latitude, @Longitude);
                    SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", sighting.UserId);
                        cmd.Parameters.AddWithValue("@Date", sighting.Date);
                        cmd.Parameters.AddWithValue("@SpecieId", sighting.SpecieId);
                        cmd.Parameters.AddWithValue("@Details", sighting.Details);
                        cmd.Parameters.AddWithValue("@ImageUrl", sighting.ImageUrl);
                        cmd.Parameters.AddWithValue("@Latitude", sighting.Latitude);
                        cmd.Parameters.AddWithValue("@Longitude", sighting.Longitude);

                        sighting.SightingId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating sighting: " + ex.Message);
                return null;
            }

            return sighting;
        }
    }
}
