using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Identity.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Web.Extensions;
using JoelScottFitness.Web.Models;
using JoelScottFitness.Web.Properties;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AccountController));

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IJSFitnessService jsfService;

        public string RootUri { get { return $"{Request.Url.Scheme}://{Request.Url.Authority}"; } }

        public AccountController(IJSFitnessService jsfService)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            this.jsfService = jsfService;
        }

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, bool showGuest = false)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ShowGuest = showGuest;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, bool showGuest = false)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ShowGuest = showGuest;
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(string.IsNullOrEmpty(returnUrl) ? @"/Home/MyPlans" : returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    ViewBag.ShowGuest = showGuest;
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string returnUrl, bool appendCustomerId = false)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.AppendCustomerId = appendCustomerId;

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Common.Models.RegisterViewModel customer, string returnUrl, bool appendCustomerId = false)
        {
            // add these properties back to the viewbag incase there is a model error
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.AppendCustomerId = appendCustomerId;

            if (ModelState.IsValid)
            {
                var user = await jsfService.GetUserAsync(customer.EmailAddress);
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "Email address is already registered");
                    customer.ConfirmEmailAddress = string.Empty;
                    return View(customer);
                }

                var newUser = new AuthUser { UserName = customer.EmailAddress, Email = customer.EmailAddress };
                var accountResult = await UserManager.CreateAsync(newUser, customer.Password);
                if (!accountResult.Succeeded)
                {
                    foreach (var error in accountResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    customer.Password = string.Empty;
                    customer.ConfirmPassword = string.Empty;
                    return View(customer);
                }

                await UserManager.AddToRoleAsync(newUser.Id, JsfRoles.User);

                // TODO remove this logic when released
                if (customer.EmailAddress.ToLower() == "blackmore__s@hotmail.com" || customer.EmailAddress.ToLower() == "joel@joelscottfitness.com")
                {
                    var identityResult = await UserManager.AddToRoleAsync(newUser.Id, JsfRoles.Admin);
                    if (!identityResult.Succeeded)
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
                        customer.Password = string.Empty;
                        customer.ConfirmPassword = string.Empty;
                        return View(customer);
                    }

                }

                var customerResult = await jsfService.CreateCustomerAsync(customer, newUser.Id);
                if (!customerResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "An error occured saving customer details please try again.");
                    customer.Password = string.Empty;
                    customer.ConfirmPassword = string.Empty;
                    return View(customer);
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code }, protocol: Request.Url.Scheme);

                var callbackViewModel = new CallbackViewModel()
                {
                    CallbackUrl = callbackUrl,
                };

                await SendConfirmAccountEmail(callbackViewModel, newUser.Email);

                if (customer.JoinMailingList)
                {
                    await UpdateMailingList(customer.EmailAddress);
                }

                return string.IsNullOrEmpty(returnUrl)
                    ? RedirectToAction("RegisterAccountConfirmation", "Account")
                    : appendCustomerId ? RedirectToLocal($"{returnUrl}?customerId={customerResult.Result}")
                                        : RedirectToLocal(returnUrl);
            }

            return View(customer);
        }

        [AllowAnonymous]
        public ActionResult RegisterAccountConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(long userId, string code)
        {
            if (userId > 0)
            {
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                return View(result.Succeeded ? "ConfirmEmail" : "Error");
            }
            return View("Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                var callbackViewModel = new CallbackViewModel()
                {
                    CallbackUrl = callbackUrl,
                };

                await SendResetPasswordEmail(callbackViewModel, model.Email);

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            // clear the users basket
            Session.Clear();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private async Task<bool> UpdateMailingList(string emailAddress)
        {
            var mailingListItemViewModel = new MailingListItemViewModel()
            {
                Active = true,
                Email = emailAddress,
            };

            return await jsfService.UpdateMailingListAsync(mailingListItemViewModel);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        private async Task<bool> SendConfirmAccountEmail(CallbackViewModel callbackViewModel, string emailAddress)
        {
            var email = this.RenderRazorViewToString("_EmailConfirmAccount", callbackViewModel, RootUri);

            return await jsfService.SendEmailAsync(Settings.Default.ConfirmAccount, email, new List<string>() { emailAddress });
        }

        private async Task<bool> SendResetPasswordEmail(CallbackViewModel callbackViewModel, string emailAddress)
        {
            var email = this.RenderRazorViewToString("_EmailResetPassword", callbackViewModel, RootUri);

            return await jsfService.SendEmailAsync(Settings.Default.ResetPassword, email, new List<string>() { emailAddress });
        }
    }
}