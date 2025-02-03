using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace SingNature.Data
{
     public class SinghtingsDAO
    {
        private static string connString = "Server=localhost;Database=SingNature;User=root;Password=Heythere12#;";

        [HttpGet]
        public static List<Sightings> GetAllSightings()
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
                            Sightings sighting = new Sightings()
                            {
                                SightingId = reader.GetInt32("SightingId"),
                                UserId = reader.GetInt32("UserId"),
                                Date = reader.GetDateTime("Date"),
                                SpecieId = reader.GetInt32("SpecieId"),
                                Details = reader["Details"] as string ?? "",
                                ImageUrl = reader["ImageUrl"] as string ?? "",
                                Latitude = reader["Latitude"].ToString(),
                                Longitude = reader["Longitude"].ToString(),
                                Status = (SightingStatus)Enum.Parse(typeof(SightingStatus), reader["Status"].ToString())
                            };
                            sightings.Add(sighting);
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