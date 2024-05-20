using EduSat.TestSeries.Service.Models.DTOs.Request;

namespace EduSat.TestSeries.Service.Provider
{
    public interface ISchoolsProvider
    {
        Task<List<School>> GetSchoolsAsync();
        Task<bool> PostSchool(School school);

        Task<List<Teacher>> GetTeachersAsync();
        Task<bool> AddTeacher(Teacher teacher);

        Task<bool> AddClass(Class clas);
        Task<List<Class>> GetClasses();

        Task<bool> AddScholarship(Scholarship scholarship);
        Task<List<Scholarship>> GetScholarships();
    }
}
