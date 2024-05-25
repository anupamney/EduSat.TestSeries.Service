namespace EduSat.TestSeries.Service.Models.DTOs.Response
{
    public class SchoolDetails
    {
        public string SchoolName { get; set; }
        public int SRN { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public int TotalStudents { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal TotalPaymentReceived { get; set; }
        public bool PaymentStatus { get; set; }
        public string AcademicYear { get; set; }
        public string District { get; set; }
        public string ClassName { get; set; }

    }
}
