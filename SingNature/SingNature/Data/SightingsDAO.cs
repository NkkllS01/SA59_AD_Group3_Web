using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace SingNature.Data
{
     public class SightingsDAO
    {
        private const string connString = "Server=localhost;Database=SingNature;User=root;Password=Heythere12#;";

        public List<Sightings> GetAllSightings()
        {
            List<Sightings> sightings = new List<Sightings>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT SightingId, UserId, Date, SpecieId, Details, ImageUrl, Latitude, Longitude, Status 
                    FROM Sightings";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("SightingId")))
                            {
                                int sightingId = reader.GetInt32("SightingId");
                                int userId = reader.GetInt32("UserId");
                                DateTime date = reader.GetDateTime("Date");
                                int specieId = reader.GetInt32("SpecieId");
                                string details = reader.IsDBNull(reader.GetOrdinal("Details")) ? "" : reader.GetString("Details");
                                string imageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl");
                                decimal latitude = reader.GetDecimal("Latitude");
                                decimal longitude = reader.GetDecimal("Longitude");
                                SightingStatus status = Enum.TryParse(reader["Status"].ToString(), out SightingStatus parsedStatus)
                                    ? parsedStatus : SightingStatus.Active;

                                sightings.Add(new Sightings(
                                    sightingId, userId, date, specieId, details ?? "", imageUrl ?? "", latitude, longitude, status
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
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT SightingId, UserId, Date, SpecieId, Details, ImageUrl, Latitude, Longitude, Status 
                    FROM Sightings
                    WHERE SightingId = @id";

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
                                    DateTime date = reader.GetDateTime("Date");
                                    int specieId = reader.GetInt32("SpecieId");
                                    string details = reader.IsDBNull(reader.GetOrdinal("Details")) ? "" : reader.GetString("Details");
                                    string imageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? "" : reader.GetString("ImageUrl");
                                    decimal latitude = reader.GetDecimal("Latitude");
                                    decimal longitude = reader.GetDecimal("Longitude");
                                    SightingStatus status = Enum.TryParse(reader["Status"].ToString(), out SightingStatus parsedStatus)
                                        ? parsedStatus : SightingStatus.Active;

                                    sighting = new Sightings(
                                        sightingId, userId, date, specieId, details, imageUrl, latitude, longitude, status
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

    }
}