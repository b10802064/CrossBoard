using CrossBorder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CrossBorder.ViewModels;
using CrossBorder.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CrossBorder.Controllers
{
    public class AccountContorller : Controller

    {
        private readonly Cross_BorderContext _context;
        public AccountContorller(Cross_BorderContext context)
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

                return LocalRedirect("~/Reports/SalesReport");
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


    }
}
