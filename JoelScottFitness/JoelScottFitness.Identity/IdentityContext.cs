using JoelScottFitness.Identity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace JoelScottFitness.Identity
{
    public class IdentityContext : IdentityDbContext<AuthUser, AuthRole, long, AuthLogin, AuthUserRole, AuthClaim>, IIdentityContext
    {
        public IdentityContext()
    : base("JoelScottFitnessDb")
        {
        }

        public IdentityContext(string dbConnection)
            : base(dbConnection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthClaim>().ToTable("AuthClaim");
            modelBuilder.Entity<AuthLogin>().ToTable("AuthLogin");
            modelBuilder.Entity<AuthRole>().ToTable("AuthRole");
            modelBuilder.Entity<AuthUser>().ToTable("AuthUser");
            modelBuilder.Entity<AuthUserRole>().ToTable("AuthUserRole");

            modelBuilder.Entity<AuthUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AuthRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AuthClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }

        public IDbSet<AuthClaim> Claims { get; set; }
        public IDbSet<AuthLogin> Logins { get; set; }
        public IDbSet<AuthUserRole> UserRoles { get; set; }
        public override IDbSet<AuthRole> Roles
        {
            get { return base.Roles; }
            set { base.Roles = value; }
        }
        public override IDbSet<AuthUser> Users
        {
            get { return base.Users; }
            set { base.Users = value; }
        }
    }
}
