using SingNature.Data;
using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SingNature.Data
{
    public class ParkDAO
    {
        private readonly string _connectionString;

        public ParkDAO()
        {
            var json = File.ReadAllText("appsettings.json");
            var jObject = JObject.Parse(json);
            _connectionString = jObject["ConnectionStrings"]["DefaultConnection"].ToString();
        }

        // Fetch all parks
        public List<Park> GetAllParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT parkId, parkName, imageUrl, parkRegion, parkDescription, openingHours latitude, longitude
                    FROM Park;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                parks.Add(new Park
                                {   
                                    ParkId = reader.GetInt32("ParkId"),
                                    ParkName = reader.GetString("ParkName"),
                                    ImageUrl = reader.GetString("ImageUrl"),
                                    ParkRegion = reader.GetString("ParkRegion"),
                                    ParkDescription = reader.GetString("ParkDescription"),
                                    OpeningHours = reader.GetString("OpeningHours"),
                                    Longitude = reader.GetDouble("Longitude"),
                                    Latitude = reader.GetDouble("Latitude")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching parks: " + ex.Message);
            }

            return parks;
        }

        // Fetch park by ID
        public Park GetParkById(int parkId)
        {
            Park park = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT parkId, parkName, openingHours, parkDescription
                    FROM Park
                    WHERE parkId = @parkId;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@parkId", parkId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                park = new Park
                                {
                                    ParkId = reader.GetInt32("parkId"),
                                    ParkName = reader.GetString("parkName"),
                                    OpeningHours = reader.GetString("openingHours"),
                                    ParkDescription = reader.GetString("parkDescription")
                                };
                            }
                            else
                            {
                                Console.WriteLine("Park not found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching park details: " + ex.Message);
            }

            return park;
        }
    }
}
