using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Models.DTOs.Response;
using Microsoft.Data.Sqlite;

namespace EduSat.TestSeries.Service.Provider
{
    public class TagDetailsProvider : ITagDetailsProvider
    {
        private readonly IConfiguration _configuration;
        public TagDetailsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(int, int)> GetSchoolDetails(int sdid)
        {
            var schoolDetails = new SchoolDetails();
            var connString =  _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);
            using var command = connection.CreateCommand();
            connection.Open();
            command.CommandText = $"SELECT teacher_id,id FROM Scholarships_details where id={sdid};";

            int teacherid=0;
            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    sdid = reader.GetInt32(reader.GetOrdinal("id"));
                    teacherid = reader.GetInt32(reader.GetOrdinal("teacher_id"));
                }
            }

            return (sdid,teacherid);
        }

        public async Task<Teacher> GetTeacher(int teacherId)
        {
            var teacher = new Teacher();

            var connString =  _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT FirstName,LastName FROM Teachers where id={teacherId};";

            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    teacher.FirstName = reader.GetString(reader.GetOrdinal("First_Name"));
                    teacher.LastName = reader.GetString(reader.GetOrdinal("Last_Name"));
                }
            }

            return teacher;
        }      
        
        public async Task<(decimal,decimal)> GetRemainingAmount(int sdid)
        {

           var TotalAmount = 0.0m;
            var paid = 0.0m;

            var connString =  _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);
            using var command = connection.CreateCommand();
            connection.Open();
            command.CommandText = $"SELECT Total_payment,Paid FROM Payment_details where scholarship_id={sdid};";

            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("Total_payment"));
                    paid = reader.GetDecimal(reader.GetOrdinal("Paid"));
                }
            }

            return (TotalAmount-paid,TotalAmount);
        }
    }
}
