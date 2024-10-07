using DevMagicMemesWebApi.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;

namespace DevMagicMemesWebApi.Identity
{
    public class IdentityCurrentUser : ICurrentUser
    {
        private readonly SignInManager<IdentityUser> _manager;

        public IdentityCurrentUser(SignInManager<IdentityUser> manager)
        {
            _manager = manager;
        }

        public bool IsAuthenticated => this.User.Identity?.IsAuthenticated ?? false;

        public ClaimsPrincipal User => _manager.Context.User;

        public string GetUserId()
        {
            return this.User.Identity?.Name ?? String.Empty;
        }

        public string GetTenantId()
        {
            return this.User.FindFirstValue("TenantId") ?? String.Empty;
        }

        public string? GetClaim(string type)
        {
            return this.User.FindFirstValue(type);
        }

        public bool HasClaim(string type, string? value)
        {
            if (value is not null)
            {
                return this.User.HasClaim(type, value);
            }
            else
            {
                return this.User.HasClaim(x => x.Type == type);
            }
        }

        public bool IsInRole(string role)
        {
            return this.User.IsInRole(role);
        }
    }
}
