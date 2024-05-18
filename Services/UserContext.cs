using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Edusat.TestSeries.Service.Domain.Constants;
using Edusat.TestSeries.Service.Domain.Enums;
using Edusat.TestSeries.Service.Domain.Models;

namespace EduSat.TestSeries.Service.Services;

/// <summary>
/// User context.
/// </summary>
[ExcludeFromCodeCoverage]
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    private ClaimsPrincipal? _claimsPrincipal;
    private UserType? _userType;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContext"/> class.
    /// </summary>
    /// <param name="contextAccessor"></param>
    public UserContext(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <summary>
    /// Gets claims of the user.
    /// </summary>
    public ClaimsPrincipal ClaimsPrincipal
    {
        get
        {
            if (_contextAccessor != null && _contextAccessor.HttpContext != null)
            {
                _claimsPrincipal ??= _contextAccessor.HttpContext.User;
            }

            return _claimsPrincipal ?? new ClaimsPrincipal();
        }
    }

    /// <summary>
    /// Gets userName.
    /// </summary>
    public string Name
    {
        get
        {
            if (ClaimsPrincipal != null && ClaimsPrincipal.Claims != null)
            {
                var nameClaim = ClaimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
                return nameClaim != null ? nameClaim.Value : string.Empty;
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Gets userId.
    /// </summary>
    public string UserId
    {
        get
        {
            return GetClaimValue("Id");
        }
    }

    /// <summary>
    /// Gets authentication Time.
    /// </summary>
    public string AuthTime
    {
        get
        {
            return GetClaimValue(ClaimConstants.AuthTime);
        }
    }

    private string GetClaimValue(string claimType)
    {
        if (ClaimsPrincipal != null && ClaimsPrincipal.Claims != null)
        {
            var nameClaim = ClaimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == claimType);
            return nameClaim != null ? nameClaim.Value : string.Empty;
        }

        return string.Empty;
    }

    /// <summary>
    /// Gets type of user.
    /// </summary>
    public UserType UserType
    {
        get
        {
            var claims = ClaimsPrincipal?.Claims;
            if (_userType == null && claims != null)
            {
                var userRoleClaims = claims.Where(x => x.Type == ClaimTypes.Role).ToList();
                if (userRoleClaims.Any())
                {
                    if (userRoleClaims.Exists(x => x.Value == ClaimConstants.AdminLogin))
                    {
                        _userType = UserType.Admin;
                    }
                    else if (userRoleClaims.Exists(x => x.Value == ClaimConstants.StaffLogin))
                    {
                        _userType = UserType.Staff;
                    }
                    else
                    {
                        _userType = UserType.None;
                    }
                }
            }

            return _userType.GetValueOrDefault();
        }
    }

    /// <summary>
    /// Gets accessToken.
    /// </summary>
    public string? AccessToken
    {
        get
        {
            return _contextAccessor?.HttpContext?.Request.Headers.Authorization;
        }
    }

    /// <summary>
    /// Gets userRole.
    /// </summary>
    /// <returns>string.</returns>
    public string? GetUserRole()
    {
        var userRoleClaims = ClaimsPrincipal?.Claims?.Where(x => x.Type == ClaimTypes.Role).ToList();
        if (userRoleClaims != null && userRoleClaims.Any())
        {
            var userRole = userRoleClaims.FirstOrDefault(a => a.Value == ClaimConstants.AdminLogin)?.Value
                ?? userRoleClaims.FirstOrDefault(a => a.Value == ClaimConstants.StaffLogin)?.Value;

            return userRole;
        }

        return string.Empty;
    }
}

