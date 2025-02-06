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
            _connectionString = jObject["ConnectionStrings"]["DefaultConnection"].ToString();
        }

        public List<Sightings> GetAllSightings()
        {
            List<Sightings> sightings = new List<Sightings>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.sightingId, s.userId, u.userName, s.date, s.specieId, sp.specieName, s.details, s.imageUrl, s.latitude, s.longitude, s.status
                    FROM Sightings s
                    JOIN Species sp ON s.specieId = sp.specieId
                    JOIN Users u ON s.userId = u.userId
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
                                SightingStatus status = Enum.TryParse(reader["Status"].ToString(), out SightingStatus parsedStatus)
                                    ? parsedStatus : SightingStatus.Active;

                                sightings.Add(new Sightings(
                                    sightingId, userId, userName, date, specieId, specieName, details ?? "", imageUrl ?? "", latitude, longitude, status
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

        public Sightings? GetSightingById(int id)
        {
            Sightings? sighting = null;
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.sightingId, s.userId, u.userName, s.date, s.specieId, sp.specieName, s.details, s.imageUrl, s.latitude, s.longitude, s.status
                    FROM Sightings s
                    JOIN Species sp ON s.specieId = sp.specieId
                    JOIN Users u ON s.userId = u.userId
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
                                    SightingStatus status = Enum.TryParse(reader["Status"].ToString(), out SightingStatus parsedStatus)
                                        ? parsedStatus : SightingStatus.Active;

                                    sighting = new Sightings(
                                        sightingId, userId, userName, date, specieId, specieName, details, imageUrl, latitude, longitude, status
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

        public List<Sightings> GetSightingsByKeyword(string keyword)
        {
            List<Sightings> sightings = new List<Sightings>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT s.sightingId, s.userId, u.userName, s.date, s.specieId, sp.specieName, s.details, s.imageUrl, s.latitude, s.longitude, s.status
                    FROM Sightings s
                    JOIN Species sp ON s.specieId = sp.specieId
                    JOIN Users u ON s.userId = u.userId
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
                                    SightingStatus status = Enum.TryParse(reader["Status"].ToString(), out SightingStatus parsedStatus)
                                        ? parsedStatus : SightingStatus.Active;

                                    sightings.Add(new Sightings(
                                        sightingId, userId, userName, date, specieId, specieName, details ?? "", imageUrl ?? "", latitude, longitude, status
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

    }
}
