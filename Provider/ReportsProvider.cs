﻿using EduSat.TestSeries.Service.Models.DTOs.Response;
using Microsoft.Data.Sqlite;

namespace EduSat.TestSeries.Service.Provider
{
    public class ReportsProvider: IReportsProvider
    {
        private readonly IConfiguration _configuration;
        public ReportsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<SchoolDetails>> GetAllSchoolDetails()
        {
            List<SchoolDetails> schoolDetails = new List<SchoolDetails>();
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT \r\n    sch.name,\r\n    sch.school_id,\r\n    tea.first_name,\r\n    tea.last_name,\r\n    tea.email_id,\r\n    sd.total_students,\r\n    pd.paid,\r\n    pd.TOTAL_PAYMENT,\r\n    pd.payment_status\r\nFROM \r\n    scholarships_details sd \r\nLEFT JOIN \r\n    schools sch ON sch.school_id = sd.school_id\r\nLEFT JOIN \r\n    teachers tea ON sd.teacher_id = tea.id\r\nLEFT JOIN \r\n    class cl ON cl.id = sd.class_id\r\nLEFT JOIN \r\n    payment_details pd ON pd.scholarship_id = sd.id;";
            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var sd= new SchoolDetails
                    {
                        SchoolName = reader.GetString(reader.GetOrdinal("name")),
                        SRN = reader.GetInt32(reader.GetOrdinal("school_id")),
                        TeacherName = reader.GetString(reader.GetOrdinal("first_name")) + reader.GetString(reader.GetOrdinal("last_name")),
                        TeacherEmail = reader.GetString(reader.GetOrdinal("email_id")),
                        TotalStudents = reader.GetInt32(reader.GetOrdinal("total_students")),
                        TotalPayment = reader.GetDecimal(reader.GetOrdinal("total_payment")),
                        TotalPaymentReceived = reader.GetDecimal(reader.GetOrdinal("paid")),
                        PaymentStatus = reader.GetBoolean(reader.GetOrdinal("payment_status"))
                     };
                     schoolDetails.Add(sd);
                }
            }
            
            return schoolDetails;
        }
    }
}