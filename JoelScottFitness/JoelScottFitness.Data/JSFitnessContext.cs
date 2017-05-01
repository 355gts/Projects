using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity;
using JoelScottFitness.Identity.Models;
using System.Data.Entity;
using System;

namespace JoelScottFitness.Data
{
    public class JSFitnessContext : IdentityContext, IJSFitnessContext
    {
        public JSFitnessContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static JSFitnessContext Create()
        {
            return new JSFitnessContext();
        }

        public EntityState SetModified(object entity)
        {
            return this.Entry(entity).State = EntityState.Modified;
        }

        public void SetValues<TEntity>(TEntity oldEntity, TEntity newEntity) where TEntity : class
        {
            this.Entry<TEntity>(oldEntity).CurrentValues.SetValues(newEntity);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<DiscountCode> DiscountCodes { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<PlanOption> PlanOptions { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchasedItem> PurchasedItems { get; set; }
    }
}
