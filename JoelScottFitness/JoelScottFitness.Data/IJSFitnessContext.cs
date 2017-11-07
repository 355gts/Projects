using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace JoelScottFitness.Data
{
    public interface IJSFitnessContext : IIdentityContext
    {
        #region context methods
        Task<int> SaveChangesAsync();

        void SetModified(object entity);

        void SetValues<TEntity>(TEntity oldEntity, TEntity newEntity) where TEntity : class;

        Database Database { get; }
        
        DbContextConfiguration Configuration { get; }

        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        #endregion

        DbSet<Address> Addresses { get; set; }

        DbSet<Blog> Blogs { get; set; }

        DbSet<BlogImage> BlogImages { get; set; }

        DbSet<Customer> Customers { get; set; }
    
        DbSet<DiscountCode> DiscountCodes { get; set; }

        DbSet<Item> Items { get; set; }

        DbSet<Plan> Plans { get; set; }

        DbSet<PlanOption> PlanOptions { get; set; }

        DbSet<Purchase> Purchases { get; set; }

        DbSet<PurchasedItem> PurchasedItems { get; set; }

        DbSet<MailingListItem> MailingList { get; set; }

        DbSet<Questionnaire> Questionnaires { get; set; }
    }
}
