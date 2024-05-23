using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface IReportsService
    {
        Task<List<SchoolDetails>> GetAllSchoolDetails();
    }
}
