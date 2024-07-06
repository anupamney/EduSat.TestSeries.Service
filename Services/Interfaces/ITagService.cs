using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface ITagService
    {
        Task<string> ResolveTags(string body, SchoolDetails recipient);
    }
}
