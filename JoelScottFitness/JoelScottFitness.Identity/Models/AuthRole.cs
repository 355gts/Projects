using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Identity.Models
{
    [Table("AuthRole")]
    public class AuthRole : IdentityRole<long, AuthUserRole>
    {
        
    }
}
