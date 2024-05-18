using EduSat.TestSeries.Service.Models.DTOs.Request;
using Microsoft.Data.Sqlite;

namespace EduSat.TestSeries.Service.Provider
{
    public class SchoolsProvider : ISchoolsProvider
    {
        private readonly IConfiguration _configuration;
        public SchoolsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<School>> GetSchoolsAsync()
        {
            var schools = new List<School>();
            var connString =  _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM schools;";

            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var school = new School
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("School_Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        AddressLine1 = reader.GetString(reader.GetOrdinal("Address_Line_1")),
                        AddressLine2 = reader.GetString(reader.GetOrdinal("Address_Line_1")),
                        City = reader.GetString(reader.GetOrdinal("Village_City")),
                        District = reader.GetString(reader.GetOrdinal("District")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        Pin = reader.GetString(reader.GetOrdinal("Pin")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        StaffId = reader.GetString(reader.GetOrdinal("Staff_Id"))                        
                    };
                    schools.Add(school);
                }
            }

            return schools;
        }

        public async Task<bool> PostSchool(School school)
        {
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            await connection.OpenAsync(); // Open the connection asynchronously

            var command = connection.CreateCommand();
            command.CommandText = @"
        INSERT INTO schools (Name, Address_Line_1, Address_Line_2, Village_City, District, State, Pin, Email, Staff_Id)
        VALUES (@Name, @AddressLine1, @AddressLine2, @City, @District, @State, @Pin, @Email, @StaffId)";

            // Add parameters to the command to prevent SQL injection
            command.Parameters.AddWithValue("@Name", school.Name);
            command.Parameters.AddWithValue("@AddressLine1", school.AddressLine1);
            command.Parameters.AddWithValue("@AddressLine2", school.AddressLine2);
            command.Parameters.AddWithValue("@City", school.City);
            command.Parameters.AddWithValue("@District", school.District);
            command.Parameters.AddWithValue("@State", school.State);
            command.Parameters.AddWithValue("@Pin", school.Pin);
            command.Parameters.AddWithValue("@Email", school.Email);
            command.Parameters.AddWithValue("@StaffId", school.StaffId);

            // Execute the command asynchronously
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // Return true if at least one row was affected (i.e., the insertion was successful)
            return rowsAffected > 0;
        }

    }
}
