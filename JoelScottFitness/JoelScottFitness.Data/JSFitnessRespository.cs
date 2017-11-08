﻿using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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

        public async Task<AsyncResult<long>> CreateCustomerAsync(Customer customer)
        {
            var existingCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);

            if (existingCustomer != null)
                return new AsyncResult<long>() { Success = false, ErrorMessage = "User already exists" };

            customer.CreatedDate = DateTime.UtcNow;
            dbContext.Customers.Add(customer);

            if (await SaveChangesAsync())
            {
                var id = existingCustomer != null ? existingCustomer.Id : customer.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<AsyncResult<long>> UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await dbContext.Customers.Include(c => c.BillingAddress).FirstOrDefaultAsync(c => c.Id == customer.Id);

            if (existingCustomer == null)
                return new AsyncResult<long>() { Success = false, ErrorMessage = $"User {customer.EmailAddress} does not exist." };

            customer.ModifiedDate = DateTime.UtcNow;

            existingCustomer.BillingAddress.AddressLine1 = customer.BillingAddress.AddressLine1;
            existingCustomer.BillingAddress.AddressLine2 = customer.BillingAddress.AddressLine2;
            existingCustomer.BillingAddress.AddressLine3 = customer.BillingAddress.AddressLine3;
            existingCustomer.BillingAddress.City = customer.BillingAddress.City;
            existingCustomer.BillingAddress.Country = customer.BillingAddress.Country;
            existingCustomer.BillingAddress.CountryCode = customer.BillingAddress.CountryCode;
            existingCustomer.BillingAddress.PostCode = customer.BillingAddress.PostCode;
            existingCustomer.BillingAddress.Region = customer.BillingAddress.Region;

            dbContext.SetValues(existingCustomer, customer);
            dbContext.SetModified(existingCustomer);

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
            var existingPlan = await dbContext.Plans
                                              .Include(p => p.Options)
                                              .FirstOrDefaultAsync(p => p.Id == plan.Id);

            if (existingPlan != null)
            {
                dbContext.SetValues(existingPlan, plan);
                dbContext.SetModified(existingPlan);

                var newOptions = plan.Options.Where(p => p.Id == 0).ToList();
                var removedOptions = existingPlan.Options.Where(ep => !plan.Options.Select(p => p.Id).ToList().Contains(ep.Id)).ToList();
                var updatedOptions = plan.Options.Where(ep => existingPlan.Options.Select(p => p.Id).ToList().Contains(ep.Id)).ToList();
                
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

        public async Task<Blog> GetBlogAsync(long id)
        {
            return await dbContext.Blogs
                                  .Include(b => b.BlogImages)
                                  .Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync(int number = 0)
        {
            var currentDate = DateTime.UtcNow;

            var blogQuery = dbContext.Blogs
                                     .Include(b => b.BlogImages)
                                     .OrderByDescending(b => b.CreatedDate);

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

        public async Task<Customer> GetCustomerDetailsAsync(string userName)
        {
            var user = await dbContext.Users
                                      .FirstOrDefaultAsync(c => c.UserName.ToLower() == userName.ToLower());

            if (user == null)
                return null;

                return await dbContext.Customers
                                      .Include(c => c.BillingAddress)
                                      .FirstOrDefaultAsync(c => c.UserId == user.Id);
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

        public async Task<PlanOption> GetPlanOptionAsync(long id)
        {
            return await dbContext.PlanOptions
                                  .FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<IEnumerable<Plan>> GetPlansByGenderAsync(Gender gender)
        {
            return await dbContext.Plans
                                  .Include(p => p.Options)
                                  .Where(p => p.TargetGender == gender)
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
            catch (DbEntityValidationException ex)
            {
                logger.Warn($"An exception occured update the database, details - '{ex.Message}'.");
            }

            return success;
        }

        public async Task<bool> UpdateMailingListAsync(MailingListItem mailingListItem)
        {
            var existingEntry = await dbContext.MailingList
                                               .Where(e => e.Email == mailingListItem.Email)
                                               .FirstOrDefaultAsync();

            if (existingEntry == null)
            {
                dbContext.MailingList.Add(mailingListItem);
            }
            else
            {
                existingEntry.Active = mailingListItem.Active;
                dbContext.SetModified(existingEntry);
            }

            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<PlanOption>> GetBasketItemsAsync(IEnumerable<long> ids)
        {
            return await dbContext.PlanOptions
                                  .Include(p => p.Plan)
                                  .Where(p => ids.Contains(p.Id))
                                  .ToListAsync();

        }

        public async Task<AuthUser> GetUserAsync(string userName)
        {
            return await dbContext.Users
                                  .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<AsyncResult<long>> SavePurchaseAsync(Purchase purchase)
        {
            dbContext.Purchases.Add(purchase);

            if (await SaveChangesAsync())
                return new AsyncResult<long>() { Success = true, Result = purchase.Id };

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<bool> UpdatePurchaseStatus(string transactionId, PurchaseStatus status)
        {
            bool success = false;

            var purchase = await dbContext.Purchases.FirstOrDefaultAsync(p => p.TransactionId == transactionId);

            if (purchase != null)
            {
                purchase.Status = status;

                if (await SaveChangesAsync())
                    success = true;
            }

            return success;
        }

        public async Task<long?> GetPurchaseIdByTransactionId(string transactionId)
        {
            var purchase = await dbContext.Purchases.FirstOrDefaultAsync(p => p.TransactionId == transactionId);

            if (purchase != null)
                return purchase.Id;

            return null;
        }

        public async Task<AsyncResult<long>> CreateOrUpdateQuestionnaireAsync(Questionnaire questionnaire)
        {
            var existingQuestionnaire = await dbContext.Questionnaires.FindAsync(questionnaire.Id);

            if (existingQuestionnaire != null)
            {
                dbContext.SetValues(existingQuestionnaire, questionnaire);
                dbContext.SetModified(existingQuestionnaire);
            }
            else
            {
                dbContext.Questionnaires.Add(questionnaire);
            }

            if (await SaveChangesAsync())
            {
                var id = existingQuestionnaire != null ? existingQuestionnaire.Id : questionnaire.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<Questionnaire> GetQuestionnaireAsync(long questionnaireId)
        {
            return await dbContext.Questionnaires.FirstOrDefaultAsync(q => q.Id == questionnaireId);
        }

        public async Task<bool> UpdatePlanStatusAsync(long planId, bool status)
        {
            var plan = await dbContext.Plans.FindAsync(planId);

            if (plan == null)
                return false;

            plan.Active = status;

            dbContext.SetModified(plan);

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateBlogStatusAsync(long blogId, bool status)
        {
            var blog = await dbContext.Blogs.FindAsync(blogId);

            if (blog == null)
                return false;

            blog.Active = status;

            dbContext.SetModified(blog);

            return await SaveChangesAsync();
        }
    }
}
