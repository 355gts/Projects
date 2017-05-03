using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Models;
using log4net;
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
        private static readonly ILog logger = LogManager.GetLogger(typeof(JSFitnessRespository));

        private readonly IJSFitnessContext dbContext;

        public JSFitnessRespository(IJSFitnessContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<AsyncResult<long>> CreateOrUpdateBlogAsync(Blog blog)
        {
            var existingBlog = await dbContext.Blogs.FindAsync(blog.Id);

            if (existingBlog != null)
            {
                dbContext.SetValues(existingBlog, blog);
                dbContext.SetModified(existingBlog);
            }
            else
            {
                dbContext.Blogs.Add(blog);
            }

            if (await SaveChangesAsync())
            {
                var id = existingBlog != null ? existingBlog.Id : blog.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<AsyncResult<long>> CreateOrUpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await dbContext.Customers.FindAsync(customer.Id);

            if (existingCustomer != null)
            {
                dbContext.SetValues(existingCustomer, customer);
                dbContext.SetValues(existingCustomer.BillingAddress, customer.BillingAddress);
                dbContext.SetModified(existingCustomer);
                dbContext.SetModified(existingCustomer.BillingAddress);
            }
            else
            {
                dbContext.Customers.Add(customer);
            }

            if (await SaveChangesAsync())
            {
                var id = existingCustomer != null ? existingCustomer.Id : customer.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<AsyncResult<long>> CreateOrUpdateDiscountCodeAsync(DiscountCode discountCode)
        {
            var existingDiscountCode = await dbContext.DiscountCodes.FindAsync(discountCode.Id);

            if (existingDiscountCode != null)
            {
                dbContext.SetValues(existingDiscountCode, discountCode);
                dbContext.SetModified(existingDiscountCode);
            }
            else
            {
                dbContext.DiscountCodes.Add(discountCode);
            }

            if (await SaveChangesAsync())
            {
                var id = existingDiscountCode != null ? existingDiscountCode.Id : discountCode.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<AsyncResult<long>> CreateOrUpdatePlanAsync(Plan plan)
        {
            var existingPlan = await dbContext.Plans.FindAsync(plan.Id);

            if (existingPlan != null)
            {
                dbContext.SetValues(existingPlan, plan);
                dbContext.SetModified(existingPlan);

                var newOptions = plan.Options.Where(p => p.Id == 0).ToList();
                var removedOptions = existingPlan.Options.Where(ep => !plan.Options.Select(p => p.Id).ToList().Contains(ep.Id)).ToList();
                var updatedOptions = existingPlan.Options.Where(ep => plan.Options.Select(p => p.Id).ToList().Contains(ep.Id)).ToList();
                
                dbContext.PlanOptions.RemoveRange(removedOptions);
                dbContext.PlanOptions.AddRange(newOptions);

                updatedOptions.ForEach(updatedOption => 
                {
                    var existingOption = existingPlan.Options.FirstOrDefault(eo => eo.Id == updatedOption.Id);

                    if (existingOption != null)
                    {
                        dbContext.SetValues(existingOption, updatedOption);
                        dbContext.SetModified(existingOption);
                    }
                });
            }
            else
            {
                dbContext.Plans.Add(plan);
            }

            if (await SaveChangesAsync())
            {
                var id = existingPlan != null ? existingPlan.Id : plan.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<AsyncResult<long>> CreatePurchaseAsync(Purchase purchase)
        {
            dbContext.Purchases.Add(purchase);

            if (await SaveChangesAsync())
                return new AsyncResult<long>() { Success = true, Result = purchase.Id };

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<bool> DeactivatePlanAsync(long id)
        {
            var plan = await dbContext.Plans.FindAsync(id);

            if (plan == null)
                return false;
            
            plan.Active = false;

            dbContext.SetModified(plan);

            return await SaveChangesAsync();
        }

        public async Task<bool> DeactivateBlogAsync(long id)
        {
            var blog = await dbContext.Blogs.FindAsync(id);

            if (blog == null)
                return false;

            blog.ActiveTo = DateTime.UtcNow;

            dbContext.SetModified(blog);

            return await SaveChangesAsync();
        }

        public async Task<Blog> GetBlogAsync(long id)
        {
            return await dbContext.Blogs.FindAsync(id);
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync(int number = 0, bool activeOnly = true)
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

        public async Task<Customer> GetCustomerDetailsAsync(long id)
        {
            return await dbContext.Customers
                                  .Include(c => c.BillingAddress)
                                  .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<DiscountCode> GetDiscountCodeAsync(long id)
        {
            return await dbContext.DiscountCodes.FindAsync(id);
        }

        public async Task<IEnumerable<DiscountCode>> GetDiscountCodesAsync()
        {
            return await dbContext.DiscountCodes
                                  .OrderBy(d => d.Code)
                                  .ToListAsync();
        }

        public async Task<Plan> GetPlanAsync(long id)
        {
            return await dbContext.Plans
                                  .Include(p => p.Options)
                                  .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Plan>> GetPlansAsync()
        {
            return await dbContext.Plans
                                  .Include(p => p.Options)
                                  .OrderBy(p => p.Name)
                                  .ToListAsync();
        }

        public async Task<Purchase> GetPurchaseAsync(long id)
        {
            return await dbContext.Purchases
                                  .Include(p => p.Items)
                                  .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Purchase>> GetPurchasesAsync(long customerId)
        {
            return await dbContext.Purchases
                                  .Include(p => p.Items)
                                  .Where(p => p.CustomerId == customerId)
                                  .OrderByDescending(p => p.PurchaseDate)
                                  .ToListAsync();
        }

        private async Task<bool> SaveChangesAsync()
        {
            bool success = false;

            try
            {
                await dbContext.SaveChangesAsync();
                success = true;
            }
            catch (DbUpdateException ex)
            {
                logger.Warn($"An exception occured update the database, details - '{ex.Message}'.");
            }

            return success;
        }
    }
}
