using Edusat.TestSeries.Service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Edusat.TestSeries.Service.Domain.Models
{
    public interface IUserContext
    {
        /// <summary>
        /// Gets claim principal from token.
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }

        /// <summary>
        /// Gets name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets userId.
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Gets authentication Time.
        /// </summary>
        public string AuthTime { get; }

        /// <summary>
        /// Gets type of user (Consumer, Staff, ...)
        /// </summary>
        public UserType UserType { get; }

        /// <summary>
        /// Gets accessToken.
        /// </summary>
        public string? AccessToken { get; }

        /// <summary>
        /// Gets userRole.
        /// </summary>
        /// <returns>string.</returns>
        public string? GetUserRole();
    }
}
