using System.Data.SqlClient;

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
    }
}