using EduSat.TestSeries.Service.Models.DTOs.Response;
using EduSat.TestSeries.Service.Provider;
using EduSat.TestSeries.Service.Services.Interfaces;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class ReportsService: IReportsService
    {
        private readonly IReportsProvider _reportsProvider;
        public ReportsService(IReportsProvider reportsProvider)
        {
            _reportsProvider = reportsProvider;
        }

        public async Task<List<SchoolDetails>> GetAllSchoolDetails()
        {
            return await _reportsProvider.GetAllSchoolDetails();
        }

    }
}
