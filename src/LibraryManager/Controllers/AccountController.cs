using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using LibraryManager.Models;
using System.Security.Claims;
using Microsoft.Net.Http.Server;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Http.Security;
using LibraryManager.Repository;

namespace LibraryManager.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserRepository userRepository)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserRepository = userRepository;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public SignInManager<ApplicationUser> SignInManager { get; private set; }
        public IUserRepository UserRepository { get; private set; }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var properties = SignInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("ExternalCallback", "Account", new { ReturnUrl = returnUrl }));
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> ExternalCallback()
        {
            ExternalLoginInfo loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login"); //TODO: Add error message
            }

            string email = (from x in loginInfo.ExternalIdentity.Claims
                            where x.Type.Contains("mail")
                            select x.Value).FirstOrDefault();

            if (email == null)
            {
                return RedirectToAction("Login"); //TODO: Add error message
            }

            //if (loginInfo..Email.EndsWith("frontiersin.org") == false)
            //{
            //    return RedirectToAction("Login");
            //}

            // Sign in the user
            var currentUser = new LibraryUser { Name = loginInfo.ExternalIdentity.Name, Email = email, ProviderKey = loginInfo.ProviderKey };

            UserRepository.CreateIfNotExists(currentUser);

            Context.Response.SignIn(currentUser.GetClaimsIdentity());
            return RedirectToAction("Index", "Books");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            Context.Response.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}