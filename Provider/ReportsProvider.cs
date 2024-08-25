using EduSat.TestSeries.Service.Models.DTOs.Response;
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
            command.CommandText = "SELECT\r\n    sd.ID,\r\n    sch.name,\r\n    tea.contact,\r\n    sch.school_id,\r\n    tea.first_name,\r\n    tea.last_name,\r\n    tea.email_id,\r\n    sd.total_students,\r\n    pd.paid,\r\n    pd.TOTAL_PAYMENT,\r\n    pd.payment_status,\r\n pd.Discount_Percent,\r\n pd.Discounted_Price,\r\n    sd.ACADEMIC_YEAR,\r\n    sch.DISTRICT,\r\n    cl.NAME as class_name,\r\n    tea.IS_PRINCIPAL\r\nFROM\r\n    scholarships_details sd\r\nLEFT JOIN\r\n    schools sch ON sch.school_id = sd.school_id\r\nLEFT JOIN\r\n    teachers tea ON sd.teacher_id = tea.id\r\nLEFT JOIN\r\n    class cl ON cl.id = sd.class_id\r\nLEFT JOIN\r\n    payment_details pd ON pd.scholarship_id = sd.id;";
            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var sd = new SchoolDetails
                    {
                        Id = !reader.IsDBNull(reader.GetOrdinal("ID")) ? reader.GetInt32(reader.GetOrdinal("ID")) : 0,
                        SchoolName = !reader.IsDBNull(reader.GetOrdinal("name")) ? reader.GetString(reader.GetOrdinal("name")) : string.Empty,
                        SRN = !reader.IsDBNull(reader.GetOrdinal("school_id")) ? reader.GetInt32(reader.GetOrdinal("school_id")) : 0,
                        TeacherFirstName = !reader.IsDBNull(reader.GetOrdinal("first_name")) ? reader.GetString(reader.GetOrdinal("first_name")) : string.Empty,
                        TeacherLastName = !reader.IsDBNull(reader.GetOrdinal("last_name")) ? reader.GetString(reader.GetOrdinal("last_name")) : string.Empty,
                        TeacherEmail = !reader.IsDBNull(reader.GetOrdinal("email_id")) ? reader.GetString(reader.GetOrdinal("email_id")) : string.Empty,
                        TeacherContact = !reader.IsDBNull(reader.GetOrdinal("contact")) ? reader.GetString(reader.GetOrdinal("contact")) : string.Empty,
                        TotalStudents = !reader.IsDBNull(reader.GetOrdinal("total_students")) ? reader.GetInt32(reader.GetOrdinal("total_students")) : 0,
                        TotalPayment = !reader.IsDBNull(reader.GetOrdinal("total_payment")) ? reader.GetDecimal(reader.GetOrdinal("total_payment")) : 0m,
                        TotalPaymentReceived = !reader.IsDBNull(reader.GetOrdinal("paid")) ? reader.GetDecimal(reader.GetOrdinal("paid")) : 0m,
                        PaymentStatus = !reader.IsDBNull(reader.GetOrdinal("payment_status")) ? reader.GetBoolean(reader.GetOrdinal("payment_status")) : false,
                        AcademicYear = !reader.IsDBNull(reader.GetOrdinal("ACADEMIC_YEAR")) ? reader.GetString(reader.GetOrdinal("ACADEMIC_YEAR")) : string.Empty,
                        District = !reader.IsDBNull(reader.GetOrdinal("DISTRICT")) ? reader.GetString(reader.GetOrdinal("DISTRICT")) : string.Empty,
                        ClassName = !reader.IsDBNull(reader.GetOrdinal("class_name")) ? reader.GetString(reader.GetOrdinal("class_name")) : string.Empty,
                        IsPrincipal = !reader.IsDBNull(reader.GetOrdinal("IS_PRINCIPAL")) ? reader.GetBoolean(reader.GetOrdinal("IS_PRINCIPAL")) : false,
                        Discount_Percent = !reader.IsDBNull(reader.GetOrdinal("Discount_Percent")) ? reader.GetInt32(reader.GetOrdinal("Discount_Percent")) : 0,
                        Discounted_Price = !reader.IsDBNull(reader.GetOrdinal("Discounted_Price")) ? reader.GetDecimal(reader.GetOrdinal("Discounted_Price")) : 0m
                    };
                    schoolDetails.Add(sd);
                }
            }
            
            return schoolDetails;
        }
    }
}
