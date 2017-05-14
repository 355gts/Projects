using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Identity.Models
{
    [Table("AuthClaim")]
    public class AuthClaim : IdentityUserClaim<long>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id
        {
            get => base.Id;
            set => base.Id = value;
        }
    }
}
