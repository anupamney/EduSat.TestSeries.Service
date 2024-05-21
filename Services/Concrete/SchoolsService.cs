using Edusat.TestSeries.Service.Domain.Models;
using EduSat.TestSeries.Service.Models.DTOs.Request;
using EduSat.TestSeries.Service.Provider;
using EduSat.TestSeries.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EduSat.TestSeries.Service.Services.Concrete
{
    public class SchoolsService: ISchoolsService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISchoolsProvider _schoolsProvider;
        private readonly IUserContext _userContext;
        public SchoolsService(UserManager<IdentityUser> userManager,ISchoolsProvider schoolsProvider,IUserContext userContext)
        {
            _userManager = userManager;
            _schoolsProvider = schoolsProvider;
            _userContext = userContext;
        }

        public async Task<List<School>> GetSchools()
        {
            return await _schoolsProvider.GetSchoolsAsync();
        }
        public async Task<bool> PostSchool(School school)
        {
            school.StaffId = _userContext.UserId;

            return await _schoolsProvider.PostSchool(school);
        }

        public async Task<List<Teacher>> GetTeachers()
        {
            return await _schoolsProvider.GetTeachersAsync();
        }
        public async Task<bool> AddTeacher(Teacher teacher)
        {
            return await _schoolsProvider.AddTeacher(teacher);
        }

        public async Task<bool> AddClass(Class clas)
        {
            return await _schoolsProvider.AddClass(clas);
        }
        public async Task<List<Class>> GetClasses()
        {
            return await _schoolsProvider.GetClasses();
        }

        public async Task<int> AddScholarship(Scholarship scholarship)
        {
            scholarship.StaffId = _userContext.UserId;
            return await _schoolsProvider.AddScholarship(scholarship);
        }

        public async Task<List<Scholarship>> GetScholarships()
        {
            return await _schoolsProvider.GetScholarships();
        }

        public async Task<bool> AddPayment(Payment payment)
        {
            return await _schoolsProvider.AddPayment(payment);
        }
        public async Task<List<Payment>> GetPayments()
        {
            return await _schoolsProvider.GetPayments();
        }

    }
}
