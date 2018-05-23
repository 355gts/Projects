using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Results;
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
            var existingBlog = await dbContext.Blogs
                                              .Include(b => b.BlogImages)
                                              .Where(b => b.Id == blog.Id)
                                              .FirstOrDefaultAsync();

            if (existingBlog != null)
            {
                dbContext.SetValues(existingBlog, blog);
                dbContext.SetModified(existingBlog);

                var newBlogImages = blog.BlogImages.Where(p => p.Id == 0).ToList();
                var removedBlogImages = existingBlog.BlogImages.Where(ep => !blog.BlogImages.Select(p => p.Id).ToList().Contains(ep.Id)).ToList();
                var updatedBlogImages = blog.BlogImages.Where(ep => existingBlog.BlogImages.Select(p => p.Id).ToList().Contains(ep.Id)).ToList();

                dbContext.BlogImages.RemoveRange(removedBlogImages);
                dbContext.BlogImages.AddRange(newBlogImages);

                updatedBlogImages.ForEach(updatedBlogImage =>
                {
                    var existingBlogImage = existingBlog.BlogImages.FirstOrDefault(eo => eo.Id == updatedBlogImage.Id);

                    if (existingBlogImage != null)
                    {
                        dbContext.SetValues(existingBlogImage, updatedBlogImage);
                        dbContext.SetModified(existingBlogImage);
                    }
                });
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

        public async Task<AsyncResult<Guid>> CreateCustomerAsync(Customer customer)
        {
            var existingCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);

            if (existingCustomer != null)
                return new AsyncResult<Guid>() { Success = false, ErrorMessage = "User already exists" };

            customer.CreatedDate = DateTime.UtcNow;
            dbContext.Customers.Add(customer);

            if (await SaveChangesAsync())
            {
                var id = existingCustomer != null ? existingCustomer.Id : customer.Id;
                return new AsyncResult<Guid>() { Success = true, Result = id };
            }

            return new AsyncResult<Guid>() { Success = false };
        }

        public async Task<AsyncResult<Guid>> UpdateCustomerAsync(Customer customer)
        {
            var existingCustomer = await dbContext.Customers.Include(c => c.BillingAddress).FirstOrDefaultAsync(c => c.Id == customer.Id);

            if (existingCustomer == null)
                return new AsyncResult<Guid>() { Success = false, ErrorMessage = $"User {customer.EmailAddress} does not exist." };

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
                return new AsyncResult<Guid>() { Success = true, Result = id };
            }

            return new AsyncResult<Guid>() { Success = false };
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

            return number > 0 
                ? await blogQuery.Take(number).ToListAsync() 
                : await blogQuery.ToListAsync();
        }

        public async Task<Customer> GetCustomerDetailsAsync(Guid id)
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

        public async Task<DiscountCode> GetDiscountCodeAsync(string code)
        {
            return await dbContext.DiscountCodes.FirstOrDefaultAsync(d => d.Code == code);
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

        public async Task<IEnumerable<PlanOption>> GetPlanOptionsAsync()
        {
            return await dbContext.PlanOptions
                                  .Include(p => p.Plan)
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

        public async Task<IEnumerable<Plan>> GetPlansByGenderAsync(Gender gender)
        {
            return await dbContext.Plans
                                  .Include(p => p.Options)
                                  .Where(p => p.TargetGender == gender)
                                  .OrderBy(p => p.Name)
                                  .ToListAsync();
        }

        public async Task<Order> GetOrderAsync(long id)
        {
            return await dbContext.Orders
                                  .Include(p => p.Customer)
                                  .Include("Customer.Plans")
                                  .Include(p => p.Items)
                                  .Include(p => p.Questionnaire)
                                  .Include("Items.Item")
                                  .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid customerId)
        {
            return await dbContext.Orders
                                  .Include(p => p.Customer)
                                  .Include(p => p.DiscountCode)
                                  .Include(p => p.Items)
                                  .Include(p => p.Questionnaire)
                                  .Include("Items.Item")
                                  .Where(p => p.CustomerId == customerId)
                                  .OrderByDescending(p => p.PurchaseDate)
                                  .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await dbContext.Orders
                                  .Include(p => p.Customer)
                                  .Include("Customer.Plans")
                                  .Include(p => p.DiscountCode)
                                  .Include(p => p.Items)
                                  .Include("Items.Item")
                                  .Include(p => p.Questionnaire)
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

        public async Task<AsyncResult<long>> SaveOrderAsync(Order order)
        {
            dbContext.Orders.Add(order);

            if (await SaveChangesAsync())
                return new AsyncResult<long>() { Success = true, Result = order.Id };

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<bool> UpdateOrderStatusAsync(string transactionId, OrderStatus status)
        {
            bool success = false;

            var order = await dbContext.Orders
                                          .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

            if (order != null)
            {
                order.Status = status;
                dbContext.SetPropertyModified(order, nameof(order.Status));

                if (await SaveChangesAsync())
                    success = true;
            }

            return success;
        }

        public async Task<bool> AssociateQuestionnaireToOrderAsync(long orderId, long questionnaireId)
        {
            bool success = false;

            var order = await dbContext.Orders.FindAsync(orderId);

            if (order != null)
            {
                order.QuestionnareId = questionnaireId;
                dbContext.SetPropertyModified(order, nameof(order.QuestionnareId));

                if (await SaveChangesAsync())
                    success = true;
            }

            return success;
        }

        public async Task<bool> AssociateQuestionnaireToPlansAsync(long orderId)
        {
            bool success = false;

            var plans = await dbContext.CustomerPlans.Where(p => p.OrderId == orderId).ToListAsync();

            if (plans != null && plans.Any())
            {
                foreach (var plan in plans)
                {
                    plan.QuestionnaireComplete = true;
                    dbContext.SetPropertyModified(plan, nameof(plan.QuestionnaireComplete));
                }

                if (await SaveChangesAsync())
                    success = true;
            }

            return success;
        }

        public async Task<Order> GetOrderByOrderIdAsync(long orderId)
        {
            return await dbContext.Orders
                                  .FirstOrDefaultAsync(p => p.Id == orderId);
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

            dbContext.SetPropertyModified(plan, nameof(plan.Active));

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateBlogStatusAsync(long blogId, bool status)
        {
            var blog = await dbContext.Blogs.FindAsync(blogId);

            if (blog == null)
                return false;

            blog.Active = status;

            dbContext.SetPropertyModified(blog, nameof(blog.Active));

            return await SaveChangesAsync();
        }

        public async Task<AsyncResult<long>> AddImageAsync(Image image)
        {
            var existingImage = await dbContext.Images.FirstOrDefaultAsync(i => i.ImagePath == image.ImagePath);

            if (existingImage != null)
                return new AsyncResult<long>() { Success = true, Result = existingImage.Id };

            dbContext.Images.Add(image);

            if (await SaveChangesAsync())
            {
                return new AsyncResult<long>() { Success = true, Result = image.Id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            var images = await dbContext.Images
                                        .ToListAsync();

            if (images == null || !images.Any())
                return Enumerable.Empty<Image>();

            return images;
        }

        public async Task<AsyncResult<long>> CreateOrUpdateImageConfigurationAsync(ImageConfiguration imageConfiguration)
        {
            var existingImageConfiguration = await dbContext.ImageConfigurations.FirstOrDefaultAsync();

            if (existingImageConfiguration != null)
            {
                existingImageConfiguration.Randomize = imageConfiguration.Randomize;
                existingImageConfiguration.SectionImage1Id = imageConfiguration.SectionImage1Id;
                existingImageConfiguration.SectionImage2Id = imageConfiguration.SectionImage2Id;
                existingImageConfiguration.SectionImage3Id = imageConfiguration.SectionImage3Id;
                existingImageConfiguration.SplashImageId = imageConfiguration.SplashImageId;

                dbContext.SetModified(existingImageConfiguration);
            }
            else
            {
                dbContext.ImageConfigurations.Add(imageConfiguration);
            }

            if (await SaveChangesAsync())
            {
                var id = existingImageConfiguration != null ? existingImageConfiguration.Id : imageConfiguration.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<ImageConfiguration> GetImageConfigurationAsync()
        {
            return await dbContext.ImageConfigurations
                                  .FirstOrDefaultAsync();
        }

        public async Task<bool> UploadCustomerPlanAsync(long planId, string sheetsUri)
        {
            var customerPlan = await dbContext.CustomerPlans.FindAsync(planId);

            if (customerPlan == null)
                return false;

            customerPlan.SheetsUri = sheetsUri;

            dbContext.SetPropertyModified(customerPlan, nameof(customerPlan.SheetsUri));

            return await SaveChangesAsync();
        }

        public async Task<OrderItem> GetOrderItemAsync(long orderItemId)
        {
            return await dbContext.OrderItems.FindAsync(orderItemId);
        }

        public async Task<bool> UpdateOrderItemAsync(OrderItem orderItem)
        {
            dbContext.SetModified(orderItem);

            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerPlan>> GetHallOfFameEntriesAsync(bool onlyEnabled = true, int? numberOfEntries = null)
        {
            var query = dbContext.CustomerPlans
                                 .Include(p => p.Item)
                                 .Include(p => p.Customer)
                                 .Where(p => p.MemberOfHallOfFame)
                                 .OrderByDescending(p => p.HallOfFameDate)
                                 .AsQueryable();

            if (onlyEnabled)
                query = query.Where(p => p.HallOfFameEnabled);

            if (numberOfEntries != null && numberOfEntries > 0)
                query = query.Take(numberOfEntries.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> UpdateHallOfFameStatusAsync(long planId, bool status)
        {
            var customerPlan = await dbContext.CustomerPlans.FindAsync(planId);

            if (customerPlan == null)
                return false;

            customerPlan.HallOfFameEnabled = status;

            dbContext.SetPropertyModified(customerPlan, nameof(customerPlan.HallOfFameEnabled));

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteHallOfFameEntryAsync(long planId)
        {
            var orderItem = await dbContext.CustomerPlans.FindAsync(planId);

            if (orderItem == null)
                return false;

            orderItem.MemberOfHallOfFame = false;
            orderItem.BeforeImage = null;
            orderItem.AfterImage = null;
            orderItem.HallOfFameEnabled = false;

            dbContext.SetPropertyModified(orderItem, nameof(orderItem.MemberOfHallOfFame));
            dbContext.SetPropertyModified(orderItem, nameof(orderItem.BeforeImage));
            dbContext.SetPropertyModified(orderItem, nameof(orderItem.AfterImage));
            dbContext.SetPropertyModified(orderItem, nameof(orderItem.HallOfFameEnabled));

            return await SaveChangesAsync();
        }

        public async Task<AsyncResult<long>> CreateOrUpdateMessageAsync(Message message)
        {
            var existingMessage = await dbContext.Messages.FindAsync(message.Id);

            if (existingMessage != null)
            {
                existingMessage.Responded = true;
                existingMessage.Response = message.Response;

                dbContext.SetModified(existingMessage);
            }
            else
            {
                dbContext.Messages.Add(message);
            }

            if (await SaveChangesAsync())
            {
                var id = existingMessage != null ? existingMessage.Id : message.Id;
                return new AsyncResult<long>() { Success = true, Result = id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await dbContext.Messages
                                  .OrderByDescending(m => m.ReceivedDate)
                                  .ToListAsync();
        }

        public async Task<Message> GetMessageAsync(long id)
        {
            return await dbContext.Messages.FindAsync(id);
        }

        public async Task<bool> DeleteImageAsync(long imageId)
        {
            var existingImage = await dbContext.Images.FindAsync(imageId);

            if (existingImage != null)
            {
                dbContext.Images.Remove(existingImage);
            }

            return await SaveChangesAsync();
        }

        public async Task<AsyncResult<long>> CreateCustomerPlanAsync(CustomerPlan customerPlan)
        {
            dbContext.CustomerPlans.Add(customerPlan);

            if (await SaveChangesAsync())
            {
                return new AsyncResult<long>() { Success = true, Result = customerPlan.Id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<AsyncResult<long>> UpdateCustomerPlanAsync(CustomerPlan customerPlan)
        {
            var existingPlan = await dbContext.CustomerPlans.FindAsync(customerPlan.Id);

            if (existingPlan != null)
            {
                dbContext.SetValues(existingPlan, customerPlan);
                dbContext.SetModified(existingPlan);
            }

            if (await SaveChangesAsync())
            {
                return new AsyncResult<long>() { Success = true, Result = customerPlan.Id };
            }

            return new AsyncResult<long>() { Success = false };
        }

        public async Task<IEnumerable<CustomerPlan>> GetCustomerPlansForOrderAsync(long orderId)
        {
            return await dbContext.CustomerPlans.Where(c => c.OrderId == orderId).ToListAsync();
        }

        public async Task<CustomerPlan> GetCustomerPlanAsync(long planId)
        {
            return await dbContext.CustomerPlans.FindAsync(planId);
        }

        public async Task<bool> UpdateHallOfFameDetailsAsync(CustomerPlan customerPlan)
        {
            dbContext.SetModified(customerPlan);

            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerPlan>> GetCustomerPlansAsync(Guid customerId)
        {
            return await dbContext.CustomerPlans
                                  .Include(i => i.Item)
                                  .Where(p => p.CustomerId == customerId)
                                  .ToListAsync();
        }

        public async Task<bool> MarkOrderCompleteAsync(long orderId)
        {
            var order = await dbContext.Orders.FindAsync(orderId);

            if (order == null)
                return false;

            order.RequiresAction = false;

            dbContext.SetPropertyModified(order, nameof(order.RequiresAction));

            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<MailingListItem>> GetMailingListAsync()
        {
            return await dbContext.MailingList.Where(m => m.Active)
                                              .ToListAsync();
        }

        public async Task<bool> DeleteBlogAsync(long blogId)
        {
            var blog = await dbContext.Blogs.FindAsync(blogId);

            if (blog != null)
            {
                dbContext.Blogs.Remove(blog);
            }

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteBlogImageAsync(long blogImageId)
        {
            var blogImage = await dbContext.BlogImages.FindAsync(blogImageId);

            if (blogImage != null)
            {
                dbContext.BlogImages.Remove(blogImage);
            }

            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteMessageAsync(long messageId)
        {
            var message = await dbContext.Messages.FindAsync(messageId);

            if (message != null)
            {
                dbContext.Messages.Remove(message);
            }

            return await SaveChangesAsync();
        }
    }
}
