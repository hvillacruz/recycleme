using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using RecycleMeMVC.Models;
using RecycleMeDomainClasses;
using RecycleMeDataAccessLayer;
using Facebook;
using RecycleMeBusinessLogicLayer;
using System.Configuration;
using RestSharp;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace RecycleMeMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext())))
        {
            UserManager.UserValidator = new UserValidator<AspNetUsers>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public AccountController(UserManager<AspNetUsers> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<AspNetUsers> UserManager { get; private set; }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string token)
        {
            ViewBag.ReturnUrl = returnUrl;
            //ViewBag.Bearer = token;
            LoginViewModel model = new LoginViewModel();
            model.Bearer = token;
            return View(model);
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);

                    string result = await Task.Run(() => ExternalToken(model.UserName, model.Password));

                    System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
                    DateTime now = DateTime.Now;
                    myCookie.Name = "RecycleAccessToken";
                    myCookie.Value = result;
                    myCookie.Expires = now.AddMinutes(14);

                    // Add the cookie.
                    Response.Cookies.Add(myCookie);


                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AspNetUsers() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    Users.Create(new UserViewModel
                    {
                        BirthDate = model.BirthDate,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserId = user.Id,
                        Avatar = "/Content/Assets/Images/avatar.jpg"

                    });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }




        public string BearerToken { get; set; }
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                if (loginInfo.Login.LoginProvider.Contains("Twitter"))
                    await StoreTwitterkAuthToken(user);
                if (loginInfo.Login.LoginProvider.Contains("Facebook"))
                    await StoreFacebookAuthToken(user);
                await SignInAsync(user, isPersistent: false);
                //await ExternalToken(user.Id);
                string result = await Task.Run(() => ExternalToken(user.Id,""));

                System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
                DateTime now = DateTime.Now;
                myCookie.Name = "RecycleAccessToken";
                myCookie.Value = result;              
                myCookie.Expires = now.AddMinutes(14);

                // Add the cookie.
                Response.Cookies.Add(myCookie);


                return RedirectToAction("Login", "Account");
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }


        public Task<string> ExternalToken(string userId,string password)
        {
            var tcs = new TaskCompletionSource<string>();
            var client = new RestClient(ConfigurationManager.AppSettings["RecycleMeAzureApi"]);


            var request = new RestRequest(String.Format("{0}", "token"), RestSharp.Method.POST);
            request.AddParameter("grant_type", "password", ParameterType.GetOrPost);
            request.AddParameter("username", userId, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
            client.ExecuteAsync(request, response =>
            {

                var result = JsonConvert.SerializeObject(response.Content);

                string jsonInput = response.Content;
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                RecycleToken token = jsonSerializer.Deserialize<RecycleToken>(jsonInput);
                tcs.TrySetResult(token.access_token);
            });
            return tcs.Task;

        }



        private async Task StoreFacebookAuthToken(AspNetUsers user)
        {
            var claimsIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            if (claimsIdentity != null)
            {
                // Retrieve the existing claims for the user and add the FacebookAccessTokenClaim
                var currentClaims = await UserManager.GetClaimsAsync(user.Id);
                var facebookAccessToken = claimsIdentity.FindAll("FacebookAccessToken").First();
                if (currentClaims.Count() <= 0)
                {
                    await UserManager.AddClaimAsync(user.Id, facebookAccessToken);
                }

            }
        }



        private async Task StoreTwitterkAuthToken(AspNetUsers user)
        {
            var claimsIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            if (claimsIdentity != null)
            {
                // Retrieve the existing claims for the user and add the FacebookAccessTokenClaim
                var currentClaims = await UserManager.GetClaimsAsync(user.Id);
                var twitterAccessToken = claimsIdentity.FindAll("urn:twitter:access_token").First();
                var twitterAccessTokenSecret = claimsIdentity.FindAll("urn:twitter:access_token_secret").First();
                if (currentClaims.Count() <= 0)
                {
                    await UserManager.AddClaimAsync(user.Id, twitterAccessToken);
                    await UserManager.AddClaimAsync(user.Id, twitterAccessTokenSecret);
                }

            }
        }


        //GET: Account/FacebookInfo
        [Authorize]
        public async Task<ActionResult> FacebookInfo()
        {
            var claimsforUser = await UserManager.GetClaimsAsync(User.Identity.GetUserId());
            var access_token = claimsforUser.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;
            var fb = new FacebookClient(access_token);
            dynamic myInfo = fb.Get("/me/friends");

            dynamic data = fb.Get("me") as Facebook.JsonObject;
            foreach (dynamic item in data)
            {

            }
            var friendsList = new List<FacebookViewModel>();
            foreach (dynamic friend in myInfo.data)
            {
                friendsList.Add(new FacebookViewModel()
                   {
                       Name = friend.name,
                       ImageURL = @"https://graph.facebook.com/" + friend.id + "/picture?type=large"
                   });
            }

            return View(friendsList);
        }



        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                var currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                if (loginInfo.Login.LoginProvider.Contains("Twitter"))
                    await StoreTwitterkAuthToken(currentUser);
                if (loginInfo.Login.LoginProvider.Contains("Facebook"))
                    await StoreFacebookAuthToken(currentUser);

                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }


                var user = new AspNetUsers()
                {
                    UserName = model.UserName,


                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        if (info.Login.LoginProvider.Contains("Twitter"))
                        {
                            await StoreTwitterkAuthToken(user);

                            var usermanager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext()));
                            var claimsforUser = usermanager.GetClaims(user.Id);
                            var access_token = claimsforUser.FirstOrDefault(x => x.Type == "urn:twitter:access_token").Value;
                            var access_token_secret = claimsforUser.FirstOrDefault(x => x.Type == "urn:twitter:access_token_secret").Value;


                            Twitter twitter = new Twitter(ConfigurationManager.AppSettings["TweetConsumerKey"],
                                                            ConfigurationManager.AppSettings["TweetConsumerSecret"], access_token, access_token_secret);

                            user.UserName = info.DefaultUserName;
                            var userResult = twitter.UserInfo(user);
                            Users.Create(userResult);
                        }
                        if (info.Login.LoginProvider.Contains("Facebook"))
                        {
                            await StoreFacebookAuthToken(user);
                            Users.Create(FB.UserInfo(user.Id));
                        }
                        await SignInAsync(user, isPersistent: false);

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
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

        private async Task SignInAsync(AspNetUsers user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
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

        private class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}