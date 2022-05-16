using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Authorization;
using Social.Core;
using Social.Core.ViewModel;
using Social.Core.ViewModel.Users;
using Social.Service.User;
using Social.Web.Models;

namespace Social.Web.Controllers
{
    public class AccountController : Controller
    {

        #region fields

        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserService _userService;
        private readonly Service.Authentication.IAuthenticationService _authenticationService;

        #endregion

        #region Ctor
        public AccountController(IUserRegistrationService userRegistrationService, IUserService userService, Service.Authentication.IAuthenticationService authenticationService)
        {
            _userRegistrationService = userRegistrationService;
            _userService = userService;
            _authenticationService = authenticationService;
        }
        #endregion

        #region login
        public IActionResult Index()
        {
            return RedirectToAction("LogIn");
        }
        [Route("login")]
        public IActionResult Login(string returnUrl = "")
        {

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = GetAppHomeUrl();
            }
            var model = new LoginModel
            {
                ReturnUrl = returnUrl,
            };
            return View(model);
        }
        [HttpPost]
        [Route("login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "/")
        {

            if (ModelState.IsValid)
            {
                var loginResult = await _userRegistrationService.ValidateUserAsync(model.Email, model.Password);
                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            var user = CommonHelper.IsValidEmail(model.Email)
                                ? await _userService.GetUserByEmailAsync(model.Email)
                                : await _userService.GetCustomerByMobileAsync(model.Email);
                            return await _userRegistrationService.SignInCustomerAsync(user, returnUrl, model.RememberMe);
                        }

                    case UserLoginResults.UserNotExist:
                        ModelState.AddModelError("", "User Not Exist");
                        break;
                    case UserLoginResults.Deleted:
                        ModelState.AddModelError("", ("User Deleted"));
                        break;
                    case UserLoginResults.NotActive:
                        ModelState.AddModelError("", "User Not Active");
                        break;
                    case UserLoginResults.NotRegistered:
                        ModelState.AddModelError("", "User Not Registered");
                        break;
                    case UserLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", "WrongCredentials");
                        break;
                }
            }

            return View(model);
        }
        #endregion

        #region register
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegistrationRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = _userService.Register(model);
            if (result.RegisterStatus == RegisterStatus.AlreadyExsist)
            {
                ModelState.AddModelError("UserName", "Register Email Exsist");
                return View(model);
            }
            else if (result.RegisterStatus != RegisterStatus.Success)
            {
                ModelState.AddModelError("failed", "Register Invalid Information");
                return View(model);
            }
            var user = CommonHelper.IsValidEmail(model.Email)
                ? await _userService.GetUserByEmailAsync(model.Email)
                : await _userService.GetCustomerByMobileAsync(model.Email);
            return await _userRegistrationService.SignInCustomerAsync(user, "/", true);

        }



        #endregion

        #region logout
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.SignOutAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region methods
        public string GetAppHomeUrl()
        {
            return Url.Action("Index", "Home");
        }

        #endregion

    }
}
