namespace EduSat.TestSeries.Service.Models.DTOs.Request
{
    public class Scholarship
    {
        public int Id { get; set; } = 0;
        public int SchoolId { get; set; }
        public int ClassId { get; set; }
        public string StaffId { get; set; }
        public int TeacherId { get; set; }
        public string AcademicYear { get; set; }
        public int NumberOfStudents { get; set; }
    }
}
