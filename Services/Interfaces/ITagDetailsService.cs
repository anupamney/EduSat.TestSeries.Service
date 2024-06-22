namespace EduSat.TestSeries.Service.Services.Interfaces
{
    public interface ITagDetailsService
    {
        //Task<string> GetTeachersFirstName(string srn);
        //Task<string> GetTeachersLastName(string srn);
        //Task<string> GetRemainingAmount(string srn);
        //Task<string> GetTotalAmount(string srn);
        string GetReceiptLink();
        string GetInvoiceLink();
    }
}
