using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Dynamic;

namespace AdPost.Data
{
    public class UserManager
    {
        private string _connectionString;
        public UserManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddUser(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users (Name,Email,PasswordHash) " +
                "VALUES(@name,@email,@passwordHash)";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public User GetUserByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT TOP 1 * FROM Users WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = command.ExecuteReader();
            if(!reader.Read())
            {
                return null;
            }
            var user = new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"]
            };
            return user;
        }
        public User GetUser(string email, string password)
        {
            var user = GetUserByEmail(email);
            if(user==null)
            {
                return null;
            }
            var isVAlid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if(!isVAlid)
            {
                return null;
            }
            return user;
        }

        
        
    }
}