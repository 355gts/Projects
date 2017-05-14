using JoelScottFitness.Identity.Models;
using System.Data.Entity;

namespace JoelScottFitness.Identity
{
    public interface IIdentityContext
    {
        IDbSet<AuthClaim> Claims { get; set; }

        IDbSet<AuthLogin> Logins { get; set; }

        IDbSet<AuthRole> Roles { get; set; }

        IDbSet<AuthUser> Users { get; set; }

        IDbSet<AuthUserRole> UserRoles { get; set; }
    }
}
