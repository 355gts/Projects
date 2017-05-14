using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Identity.Models
{
    [Table("AuthLogin")]
    public class AuthLogin : IdentityUserLogin<long>
    {
    }
}
