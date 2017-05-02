using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity;
using JoelScottFitness.Identity.Models;
using System.Data.Entity;
using System;
using log4net;
using System.Collections.Generic;

namespace JoelScottFitness.Data
{
    public class JSFitnessContext : IdentityContext, IJSFitnessContext
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JSFitnessContext));

        public JSFitnessContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new NullDatabaseInitializer<JSFitnessContext>());

            Database.Log = logentry =>
            {

                logger.Debug(logentry);
#if DEBUG
                System.Diagnostics.Debug.WriteLine(logentry);
#endif
            };
            
            Configuration.ProxyCreationEnabled = false;
        }

        public static JSFitnessContext Create()
        {
            return new JSFitnessContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));
            
            //modelBuilder.HasDefaultSchema(Settings.Default.DefaultSchema);
            //modelBuilder.Properties<string>().Configure(c => c.HasColumnType("VARCHAR2"));
            
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }
    
        public virtual void CommitTransaction()
        {
            Database.CurrentTransaction?.Commit();
        }

        public virtual void RollbackTransaction()
        {
            Database.CurrentTransaction?.Rollback();
        }
        
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetAdded(object entity)
        {
            Entry(entity).State = EntityState.Added;
        }
        
        public virtual void SetPropertyModified(object entity, string propertyName)
        {
            Entry(entity).Property(propertyName).IsModified = true;
        }

        public virtual void SetValues(object oldEntity, object newEntity)
        {

            Entry(oldEntity).CurrentValues.SetValues(newEntity);
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
