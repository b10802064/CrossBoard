using CrossBorder.Data;
using Microsoft.AspNetCore.Mvc;
using CrossBorder.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using CrossBorder.Models;
using cross_border.ViewModels;

namespace cross_border.Controllers
{
    public class AccountController : Controller
    {
        private readonly Cross_BorderContext _context;
        public AccountController (Cross_BorderContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                // 驗證使用者帳密
                ApplicationUser user = await AuthenticateUser(loginVM);
                //ApplicationUser user = await _accountServices.AuthenticateUser(loginVM);
                //失敗
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "帳號密碼有錯");
                    return View(loginVM);
                }
                //成功，通過帳比對，以下開始建立授權
                //通過以上帳密比對成立後, 以下開始建立授權
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    //new Claim(ClaimTypes.Role, "Administrator") // 如果要有「群組、角色、權限」，可以加入這一段  
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                    );

                return LocalRedirect("~/Products/Productmain");
            }
            return View(loginVM);
        }



        private async Task<ApplicationUser> AuthenticateUser(LoginViewModel loginVM)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.CusdtomerName.ToUpper() == loginVM.UserName.ToUpper() && u.Password == loginVM.Password);
            if (user != null)
            {

                var userInfo = new ApplicationUser
                {
                    Name = user.CusdtomerName,
                    Email = user.Email,
                };

                return userInfo;
            }
            else
            {
                return null;
            }

        }
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }


        //註冊帳號
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                //ViewModel => Data Model
                Customer user = new Customer
                {
                    CustomerId = Guid.NewGuid().ToString(),
                    CusdtomerName = registerVM.UserName,
                    Password = registerVM.Password,
                    Email = registerVM.Email,
                };

                _context.Customers.Add(user);
                _context.SaveChanges();

                ViewData["Title"] = "帳號註冊";
                ViewData["Message"] = "使用者帳號註冊成功!";  //顯示訊息

                return View("~/Views/Shared/ResultMessage.cshtml");
            }

            return View(registerVM);
        }



    }
}
