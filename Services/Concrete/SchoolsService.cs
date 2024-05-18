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
    }
}
