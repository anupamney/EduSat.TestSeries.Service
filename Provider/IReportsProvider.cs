using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Provider
{
    public interface IReportsProvider
    {
        Task<List<SchoolDetails>> GetAllSchoolDetails();
    }
}
