using Microsoft.AspNetCore.Identity;
using EduSat.TestSeries.Service.Models.DTOs.Auth.Request;
using EduSat.TestSeries.Service.Models.DTOs.Auth.Response;

namespace EduSat.TestSeries.Service.Services.Interfaces;

public interface IJwtService
{
    Task<AuthResult> GenerateToken(IdentityUser user, string role);
    Task<RefreshTokenResponseDTO> VerifyToken(TokenRequestDTO tokenRequest);

}
