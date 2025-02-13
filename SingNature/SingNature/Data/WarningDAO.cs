using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using static SingNature.Models.Warning;

namespace SingNature.Data
{
    public class WarningDAO
    {
        private readonly string _connectionString;

        public WarningDAO()
        {
            var json = File.ReadAllText("appsettings.json");
            var jObject = JObject.Parse(json);
            _connectionString = jObject["ConnectionStrings"]["DefaultConnection"].ToString();
        }

        // Get all warnings
        public List<Warning> GetAllWarnings()
        {
            List<Warning> warnings = new List<Warning>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT WarningId, Source, SightingId, Cluster, AlertLevel, Date, Description, Specie
                    FROM warning;"; 

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                warnings.Add(new Warning
                                {   
                                    WarningId = reader.GetInt32("WarningId"),
                                    Source = Enum.TryParse<WarningSource>(reader.GetString("Source"), true, out var source) ? source : WarningSource.SIGHTING,
                                    SightingId = reader.IsDBNull(reader.GetOrdinal("SightingId")) ? null : reader.GetInt32("SightingId"),
                                    Cluster = reader.IsDBNull(reader.GetOrdinal("Cluster")) ? null : reader.GetString("Cluster"),
                                    AlertLevel = reader.IsDBNull(reader.GetOrdinal("AlertLevel")) ? null : reader.GetString("AlertLevel"),
                                    Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetDateTime("Date").ToString("yyyy-MM-dd"),
                                    Description = reader.GetString("Description"),
                                    Specie = reader.IsDBNull(reader.GetOrdinal("Specie")) ? null : reader.GetString("Specie")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching warnings: " + ex.Message);
            }

            return warnings;
        }

        // Get warning by id
        public Warning GetWarningById(int warningId)
        {
            Warning warning = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = @"
                    SELECT WarningId, Source, SightingId, Cluster, AlertLevel, Date, Description, Specie
                    FROM warning
                    WHERE WarningId = @WarningId;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@WarningId", warningId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                warning = new Warning
                                {
                                    WarningId = reader.GetInt32("WarningId"),
                                    Source = Enum.TryParse<WarningSource>(reader.GetString("Source"), true, out var source) ? source : WarningSource.DENGUE,
                                    SightingId = reader.IsDBNull(reader.GetOrdinal("SightingId")) ? null : reader.GetInt32("SightingId"),
                                    Cluster = reader.IsDBNull(reader.GetOrdinal("Cluster")) ? null : reader.GetString("Cluster"),
                                    AlertLevel = reader.IsDBNull(reader.GetOrdinal("AlertLevel")) ? null : reader.GetString("AlertLevel"),
                                    Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? null : reader.GetDateTime("Date").ToString("yyyy-MM-dd"),
                                    Description = reader.GetString("Description"),
                                    Specie = reader.IsDBNull(reader.GetOrdinal("Specie")) ? null : reader.GetString("Specie")
                                };
                            }
                            else
                            {
                                Console.WriteLine("Warning not found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching warning details: " + ex.Message);
            }

            return warning;
        }
    }
}

