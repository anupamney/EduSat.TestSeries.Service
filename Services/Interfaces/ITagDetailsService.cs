using EduSat.TestSeries.Service.Models.DTOs.Response;

namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface ITagDetailsService
    {
        Task<string> GetReceiptLinkAsync(SchoolDetails recipient);
        Task<string> GetInvoiceLinkAsync(SchoolDetails recipient);
    }
}
