using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace JoelScottFitness.Data
{
    public class JSFitnessRespository : IJSFitnessRepository
    {
        private readonly IJSFitnessContext dbContext;

        public JSFitnessRespository(IJSFitnessContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            this.dbContext = dbContext;
        }

        public async Task<long> CreateOrUpdateBlog(Blog blog)
        {
            throw new NotImplementedException();
        }

        public async Task<long> CreateOrUpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<long> CreateOrUpdateDiscountCode(DiscountCode discountCode)
        {
            throw new NotImplementedException();
        }

        public async Task<long> CreateOrUpdatePlan(Plan plan)
        {
            throw new NotImplementedException();
        }

        public async Task<long> CreatePurchase(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeactivatePlan(long id)
        {
            var plan = await dbContext.Plans.FindAsync(id);

            if (plan == null)
                return false;
            
            plan.Active = false;

            dbContext.SetModified(plan);

            return await SaveChangesAync();
        }

        public async Task<bool> DeleteBlog(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Blog> GetBlog(long id)
        {
            return await dbContext.Blogs.FindAsync(id);
        }

        public async Task<IEnumerable<Blog>> GetBlogs(int number = 0, bool activeOnly = true)
        {
            var currentDate = DateTime.UtcNow;

            var blogQuery = dbContext.Blogs
                                     .Where(b => !activeOnly 
                                        || (b.ActiveFrom <= currentDate 
                                        && currentDate <= b.ActiveTo))
                                     .OrderByDescending(b => b.ActiveFrom);

            if (number > 0)
                blogQuery.Take(number);

            return await blogQuery.ToListAsync();
        }

        public async Task<Customer> GetCustomerDetails(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<DiscountCode> GetDiscountCode(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DiscountCode>> GetDiscountCodes()
        {
            throw new NotImplementedException();
        }

        public async Task<Plan> GetPlan(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Plan>> GetPlans()
        {
            throw new NotImplementedException();
        }

        public async Task<Purchase> GetPurchase(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Purchase>> GetPurchases(long customerId)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> SaveChangesAync()
        {
            bool success = false;

            try
            {
                await dbContext.SaveChangesAsync();
                success = true;
            }
            catch (DbUpdateException ex)
            {
                //TODO log out here
            }

            return success;
        }
    }
}
