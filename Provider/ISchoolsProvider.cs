using EduSat.TestSeries.Service.Models.DTOs.Request;

namespace EduSat.TestSeries.Service.Provider
{
    public interface ISchoolsProvider
    {
        Task<List<School>> GetSchoolsAsync();
        Task<bool> PostSchool(School school);
    }
}
