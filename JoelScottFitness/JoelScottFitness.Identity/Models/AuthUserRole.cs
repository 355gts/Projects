using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Identity.Models
{
    [Table("AuthUserRole")]
    public class AuthUserRole : IdentityUserRole<long>
    {
    }
}
