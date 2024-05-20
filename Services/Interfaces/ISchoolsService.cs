using EduSat.TestSeries.Service.Models.DTOs.Request;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface ISchoolsService
    {
        Task<bool> PostSchool(School school);
        Task<List<School>> GetSchools();
        Task<bool> AddTeacher(Teacher teacher);
        Task<List<Teacher>> GetTeachers();
        Task<bool> AddClass(Class clas);
        Task<List<Class>> GetClasses();
        Task<bool> AddScholarship(Scholarship scholarship);
        Task<List<Scholarship>> GetScholarships();
    }
}
