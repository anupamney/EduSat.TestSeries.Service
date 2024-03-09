using Microsoft.AspNetCore.Identity;
using EduSat.TestSeries.Service.Dtos.Auth;
using EduSat.TestSeries.Service.Dtos.Auth.Request;
using EduSat.TestSeries.Service.Dtos.Auth.Response;

namespace EduSat.TestSeries.Service.Services;

public interface IJwtService
{
    Task<AuthResult> GenerateToken(IdentityUser user);
    Task<RefreshTokenResponseDTO> VerifyToken(TokenRequestDTO tokenRequest);
    
}
