using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Reflection.Metadata;

namespace AdPost.Data
{
    public class AdManager
    {
        private string _connectionString;
        public AdManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddAd(string phonenumber, string description, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Ads (Date, PhoneNumber, Description, UserId) " +
                "VALUES (CURRENT_TIMESTAMP, @phonenumber, @description, @userId)";
            command.Parameters.AddWithValue("@phonenumber", phonenumber);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public List<Ad> GetAdsWithUserName()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT a.*, u.Name FROM Ads a " +
                "LEFT JOIN Users u " +
                "ON a.UserId = u.Id";
            connection.Open();
            var ads = new List<Ad>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var ad = new Ad
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserId"],
                    Name = (string)reader["Name"]
                };
                ads.Add(ad);
            }
            connection.Close();
            return ads;

        }
        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Ads WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public List<Ad> GetAdsForMyAccount(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT a.*, u.Name FROM Ads a " +
                "LEFT JOIN Users u " +
                "ON a.UserId = u.Id " +
                "WHERE a.UserId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var ads = new List<Ad>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var ad = new Ad
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserId"],
                    Name = (string)reader["Name"]
                };
                ads.Add(ad);
            }
            connection.Close();
            return ads;
        }
    }
}