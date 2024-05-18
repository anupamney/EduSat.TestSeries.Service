using EduSat.TestSeries.Service.Models.DTOs.Request;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface ISchoolsService
    {
        Task<bool> PostSchool(School school);
        Task<List<School>> GetSchools();
    }
}
