using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Provider
{
    public interface ITagDetailsProvider
    {
        Task<Teacher> GetTeacher(int teacherId);
        Task<(int,int)> GetSchoolDetails(int srn);
        Task<(decimal,decimal)> GetRemainingAmount(int sdid);
    }
}
