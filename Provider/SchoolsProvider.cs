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

        public async Task<List<Teacher>> GetTeachersAsync()
        {
            var teachers = new List<Teacher>();
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM teachers;";

            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var teacher = new Teacher
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("First_Name")),
                        LastName = reader.GetString(reader.GetOrdinal("Last_Name")),
                        Email = reader.GetString(reader.GetOrdinal("Email_ID")),
                        Mobile = reader.GetString(reader.GetOrdinal("Contact"))
                    };
                    teachers.Add(teacher);
                }
            }

            return teachers;
        }

        public async Task<bool> AddTeacher(Teacher teacher)
        {
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            await connection.OpenAsync(); // Open the connection asynchronously

            var command = connection.CreateCommand();
            command.CommandText = @"Insert into teachers (First_Name, Last_Name, Contact, Email_ID)
                                    VALUES (@FirstName, @LastName, @Mobile, @Email)";
            command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
            command.Parameters.AddWithValue("@LastName", teacher.LastName);
            command.Parameters.AddWithValue("@Mobile", teacher.Mobile);
            command.Parameters.AddWithValue("@Email", teacher.Email);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> AddClass(Class clas)
        {
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            await connection.OpenAsync(); // Open the connection asynchronously

            var command = connection.CreateCommand();
            command.CommandText = @"Insert into class (Class_Name)
                                    VALUES (@ClassName)";
            command.Parameters.AddWithValue("@ClassName", clas.Name);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<List<Class>> GetClasses()
        {
            var classes = new List<Class>();
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM class;";

            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var clas = new Class
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name"))
                    };
                    classes.Add(clas);
                }
            }

            return classes;
        }

        public async Task<int> AddScholarship(Scholarship scholarship)
        {
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            await connection.OpenAsync(); // Open the connection asynchronously

            var command = connection.CreateCommand();
            command.CommandText = @"Insert into scholarships_details (School_Id, Class_Id, Staff_Id,Teacher_Id,Academic_Year,Total_Students)
                                    VALUES (@School_Id,@Class_Id,@Staff_Id,@Teacher_Id,@Academic_Year,@Total_Students)";

            command.Parameters.AddWithValue("@School_Id", scholarship.SchoolId);
            command.Parameters.AddWithValue("@Class_Id", scholarship.ClassId);
            command.Parameters.AddWithValue("@Staff_Id", scholarship.StaffId);
            command.Parameters.AddWithValue("@Teacher_Id", scholarship.TeacherId);
            command.Parameters.AddWithValue("@Academic_Year", scholarship.AcademicYear);
            command.Parameters.AddWithValue("@Total_Students", scholarship.NumberOfStudents);

            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected > 0)
            {
                   command.CommandText = @"SELECT last_insert_rowid()";
                return  (int)(long)await command.ExecuteScalarAsync();
            }

            return rowsAffected;
        }

        public async Task<List<Scholarship>> GetScholarships()
        {
            var scholarships = new List<Scholarship>();
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM scholarships_details;";

            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var scholarship = new Scholarship
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        SchoolId = reader.GetInt32(reader.GetOrdinal("School_Id")),
                        ClassId = reader.GetInt32(reader.GetOrdinal("Class_Id")),
                        StaffId = reader.GetString(reader.GetOrdinal("Staff_Id")),
                        TeacherId = reader.GetInt32(reader.GetOrdinal("Teacher_Id")),
                        AcademicYear = reader.GetString(reader.GetOrdinal("Academic_Year")),
                        NumberOfStudents = reader.GetInt32(reader.GetOrdinal("Total_Students"))
                    };
                    scholarships.Add(scholarship);
                }
            }

            return scholarships;
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            await connection.OpenAsync(); // Open the connection asynchronously

            var command = connection.CreateCommand();
            command.CommandText = @"Insert into payment_details (Scholarship_Id, Total_payment, Paid,Payment_Status)
                                    VALUES (@Scholarship_Id,@Total_payment,@Paid,@Payment_Status)";

            command.Parameters.AddWithValue("@Scholarship_Id", payment.ScholarshipId);
            command.Parameters.AddWithValue("@Total_payment", payment.TotalPayment);
            command.Parameters.AddWithValue("@Paid", payment.AmountPaid);
            command.Parameters.AddWithValue("@Payment_Status", payment.PaymentStatus);
            
            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<List<Payment>> GetPayments()
        {
            var payments = new List<Payment>();
            var connString = _configuration.GetConnectionString("Default");
            using var connection = new SqliteConnection(connString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM payment_details;";

            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var payment = new Payment
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        ScholarshipId = reader.GetInt32(reader.GetOrdinal("Scholarship_Id")),
                        TotalPayment = reader.GetDecimal(reader.GetOrdinal("Total_payment")),
                        AmountPaid = reader.GetDecimal(reader.GetOrdinal("Paid")),
                        PaymentStatus = reader.GetBoolean(reader.GetOrdinal("Payment_Status"))
                    };
                    payments.Add(payment);
                }
            }

            return payments;
        }
    }
}
