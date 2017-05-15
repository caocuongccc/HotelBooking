using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Einvoice_Customer.Models;
using System.Web.Security;
using System.Configuration;
using Einvoice_Customer.Service;
using HDDT_FURAMA;

namespace Einvoice_Customer.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager {
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
        [OutputCache(NoStore=true, Duration = 0, VaryByParam= "None")] 
        public ActionResult Login(string returnUrl, string mess)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Message = mess;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoLogin(DoLoginViewModel model)
        {
            bool isExist = false;
            using (var chanel = new ChanelFactory())
            {
                isExist = await chanel.CheckCusExist<bool>(model.CustomerCode.Trim());

                if (isExist)
                {
                    var result = await chanel.Login<bool>(model.CustomerCode.Trim(), model.Password);
                    if (result)
                    {
                        FormsAuthentication.SetAuthCookie(model.CustomerCode, false);
                        return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { IsSuccess = false, Error = Einvoice_Customer.Language.Resource.ErrorPass }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { IsSuccess = false, Error = Einvoice_Customer.Language.Resource.usernotExist }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            bool isExist = false;
            using (var chanel = new ChanelFactory())
            {
                isExist = await chanel.CheckCusExist<bool>(model.CustomerCode.Trim());

                if (isExist)
                {
                    var result = await chanel.Login<bool>(model.CustomerCode.Trim(), model.Password);
                    if (result)
                    {
                        FormsAuthentication.SetAuthCookie(model.CustomerCode, false);
                        if (model.CustomerCode == "admin")
                        {
                            return RedirectToAction("About", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "EinvoiceCustomer");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", Einvoice_Customer.Language.Resource.accountnotExist);
                    }
                }
                else // không có trong data, kiểm tra trên hệ thống
                {
                    ModelState.AddModelError("", Einvoice_Customer.Language.Resource.usernotExist);
                }
            }
            return View(model);
        }

        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindAsync(model.Email, model.Password);
        //        if (user != null)
        //        {
        //            await SignInAsync(user, model.RememberMe);
        //            return RedirectToLocal(returnUrl);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Invalid username or password.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

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
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            IdentityResult result;
            try
            {
                result = await UserManager.ConfirmEmailAsync(userId, code);
            }
            catch (InvalidOperationException ioe)
            {
                // ConfirmEmailAsync throws when the userId is not found.
                ViewBag.errorMessage = ioe.Message;
                return View("Error");
            }

            if (result.Succeeded)
            {
                return View();
            }

            // If we got this far, something failed.
            AddErrors(result);
            ViewBag.errorMessage = Einvoice_Customer.Language.Resource.errorConfirmEmail;
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
            bool isExist = false;
            using (var chanel = new ChanelFactory())
            {
                isExist = await chanel.CheckCusExist<bool>(model.Account);
                // có trong data nên kiểm tra user & pass
                if (isExist)
                {
                    string pass = GenPass();
                    Customer cus = await GetByCusCode(model.Account);
                    PortalServiceModel.SetCustomerPasswordRequest modelRequest = new PortalServiceModel.SetCustomerPasswordRequest()
                    {
                        username = model.Account,
                        password = pass
                    };
                    var result = await chanel.PostObjectToWebApi<string>("Portal", "UpdatePassword", modelRequest);
                    var emailCus = ConvertCustomerXMLToCustomerModel(cus).Email;
                    if (result.CompareTo("OK") == 0 && emailCus != "")
                    {
                        var title = "[ " + ConfigurationManager.AppSettings["EmailCompany"] + " ]" + Einvoice_Customer.Language.Resource.alertNewPass;
                        var content = "<br/>" + Einvoice_Customer.Language.Resource.EmailTitle + "<br/>" + "InterContinental Danang Sun Peninsula Resort "
                            + Einvoice_Customer.Language.Resource.alertNewPass1
                            + pass + "<br/>"
                            + Einvoice_Customer.Language.Resource.alertNewPass2
                            + " " 
                            + ConfigurationManager.AppSettings["UrlPortal"] 
                            + " " 
                            + Einvoice_Customer.Language.Resource.alertNewPass3 + "<br/><br/>"+
                            Einvoice_Customer.Language.Resource.EmailFooter;
                        await this.EmailService.SendEmailAsync(emailCus, title,content);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", Einvoice_Customer.Language.Resource.cantSendpass);
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", Einvoice_Customer.Language.Resource.alertCheckagain);
                    return View();
                }
            }
            // If we got this far, something failed, redisplay form
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
            if (code == null) 
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", Einvoice_Customer.Language.Resource.userNotFound);
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
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
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
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
                message == ManageMessageId.ChangePasswordSuccess ? Einvoice_Customer.Language.Resource.changedpass
                : message == ManageMessageId.SetPasswordSuccess ? Einvoice_Customer.Language.Resource.createdpass
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
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
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
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

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
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
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
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
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
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");
                        
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
           // AuthenticationManager.SignOut();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Profile
        [AllowAnonymous]
        public async Task<ActionResult> Profile()
        {
            CustomerViewModel c = new CustomerViewModel();

            string cusCode = User.Identity.Name;
            Customer cus = await GetByCusCode(cusCode);

            var cModel = ConvertCustomerXMLToCustomerModel(cus);

            c.CusCode = cModel.Code;
            c.Name = cModel.Name;// xmlDoc.FirstChild["name"].InnerXml.Replace("<![CDATA[", "").Replace("]]>", "");
            c.Address = cModel.Address;// xmlDoc.FirstChild["address"].InnerXml.Replace("<![CDATA[", "").Replace("]]>", "");
            c.Phone = cModel.Phone;// xmlDoc.FirstChild["phone"].InnerXml;
            c.Taxcode = cModel.TaxCode;// xmlDoc.FirstChild["taxcode"].InnerXml;
            c.Email = cModel.Email;// xmlDoc.FirstChild["email"].InnerXml;

            return View(c);
        }

        public async Task<ActionResult> Edit()
        {
            string cusCode = User.Identity.Name;
            var cus = await GetByCusCode(cusCode);
            var cModel = ConvertCustomerXMLToCustomerModel(cus);

            return View(cModel);

           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var cus = new Customer();
            if (cus != null)
            {
                cus.Code = model.Code;
                cus.Name = model.Name;
                cus.TimeEdit = DateTime.UtcNow;
                cus.TaxCode = model.TaxCode;
                cus.XML = GeneralXML_Customer(model);
                await UpdateCus(cus);
                return RedirectToAction("Profile");
            }
            return View(model);
        }
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel cpvm = new ChangePasswordViewModel();
            cpvm.UserName = User.Identity.GetUserName();
            return View(cpvm);
        }

        public ActionResult ChangePasswordFirst()
        {
            ChangePasswordViewModel cpvm = new ChangePasswordViewModel();
            cpvm.UserName = User.Identity.GetUserName();
            return View(cpvm);
        }

        // Kiểm tra mật khẩu cũ nhập lại khi đổi mật khẩu có đúng không
        // Hàm này dùng kết hợp với thuộc tính Remote trong AccountViewModel
      

        [HttpPost]
        public async Task<ActionResult> SaveChangePassword(ChangePasswordViewModel cpvm)
        {
            var result = "";
            using (var chanel = new ChanelFactory())
            {
                PortalServiceModel.SetCustomerPasswordRequest model = new PortalServiceModel.SetCustomerPasswordRequest()
                {
                    username = cpvm.UserName,
                    password = cpvm.NewPassword
                };
                result = await chanel.PostObjectToWebApi<string>("Portal", "UpdatePassword", model);
                if (result.CompareTo("OK") == 0)
                {
                    return View("ChangePasswordOK");
                }
            }
            return View("ChangePasswordNotOK");
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
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                //if (_signInManager != null)
                //{
                //    _signInManager.Dispose();
                //    _signInManager = null;
                //}
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

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
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

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
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
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
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