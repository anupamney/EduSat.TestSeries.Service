using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduSat.TestSeries.Service.Models;

namespace EduSat.TestSeries.Service.Data;

public class ApiDbContext : IdentityDbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> opt) : base(opt)
    {

    }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
}
