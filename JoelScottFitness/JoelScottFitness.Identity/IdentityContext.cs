using JoelScottFitness.Identity.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JoelScottFitness.Identity
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext()
    :       base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public IdentityContext(string dbConnection, bool throwIfV1Schema = false)
            : base(dbConnection, throwIfV1Schema: throwIfV1Schema)
        {
        }

        //public static IdentityContext Create()
        //{
        //    return new IdentityContext();
        //}
    }
}
