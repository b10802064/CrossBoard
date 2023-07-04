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
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using CrossBorder.MyClass;

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

                return LocalRedirect("~/Products/ProductList");
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
                Random random = new Random();

                // 生成四個隨機數字
                //string number1 = random.Next().ToString();
                //string number2 = random.Next().ToString();
                //string number3 = random.Next().ToString();
                //string number4 = random.Next().ToString();
                //string number5 = number1 + number2 + number3 + number4;
                //ViewModel => Data Model
                Customer user = new Customer
                {
                    CustomerId = Guid.NewGuid().ToString(),
                    CusdtomerName = registerVM.UserName,
                    Password = registerVM.Password,
                    Email = registerVM.Email,
                };
                //Send_eMail(registerVM.Email, "測試", "驗證碼:"+ number5);
                Net.LineNotify("有帳號註冊 :\n"+ registerVM.UserName);
                _context.Customers.Add(user);
                _context.SaveChanges();
                ViewData["Title"] = "帳號註冊";
                ViewData["Message"] = "使用者帳號註冊成功!";  //顯示訊息

                return View("~/Views/Shared/ResultMessage_true.cshtml");
            }

            return View(registerVM);
        }






        public static async void Send_eMail(string mailto ,string subject, string body)
        { //寄發 eMail 信箱
            /* [ C# 開發隨筆 ] Gmail SMTP發信 ~無法採用低安全性登入後的修正~
             * https://dotblogs.com.tw/anmlab/2022/06/14/153437
            */
            //bool IsSend = false;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("90908787zz@gmail.com", "行翔"); //前面是發信email後面是顯示的名稱

            mail.To.Add(mailto);

            mail.Priority = MailPriority.Normal; //優先權等級
            mail.Subject = subject; //標題
            mail.Body = body; //內文
            mail.IsBodyHtml = true; //內容使用html
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                { //設定gmail的smtp (這是google的)
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("90908787zz", "itpwlkxrciomtujv"); //gmail的帳號密碼
                    smtp.EnableSsl = true; //開啟ssl
                                           //smtp.Send(mail);
                    await smtp.SendMailAsync(mail); //發送郵件
                }
                //if (IsSend)
                //{
                //}
            }
            catch (Exception)
            {
                //Net.Event_Message(13, "Turnkey", "網路發生異常，請洽知服務人員處理", "Event,Mess013");
            }
            mail.Dispose();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserFeedBack(string feedback)
        {
            Net.LineNotify("用戶反饋 :\n" + feedback);

            ViewData["Title"] = "用戶反饋";
            ViewData["Message"] = "用戶反饋成功!";  //顯示訊息

            return View("~/Views/Shared/ResultMessage_true.cshtml");
        }

        public IActionResult CEdit()
        {
            string loggedInUsername = User.Identity.Name;
            var customer = _context.Customers.FirstOrDefault(m => m.CusdtomerName == loggedInUsername);
            if (customer == null)
            {
                return View("Error");
            }

            return View(customer);
        }

        // POST: Member/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CEdit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Attach(customer);
                _context.Entry(customer).Property(m => m.CusdtomerName).IsModified = true;
                _context.Entry(customer).Property(m => m.Email).IsModified = true;
                _context.Entry(customer).Property(m => m.Password).IsModified = true;

                if (customer.Password == null)
                {
                    ModelState.AddModelError(string.Empty, "密碼未輸入");
                    return View(customer);
                }
                _context.SaveChanges();

                // 執行登出操作
                Task.Run(async () => await HttpContext.SignOutAsync()).Wait();

                // 使用新的會員資訊進行身份驗證並登入
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, customer.CusdtomerName)

                    // 添加其他需要的 Claim
                };

                var identity = new ClaimsIdentity(claims, "MemberIdentity");
                var principal = new ClaimsPrincipal(identity);

                Task.Run(async () => await HttpContext.SignInAsync(principal)).Wait();

                // 重新導向到某個頁面或執行其他適當的處理
                return RedirectToAction("Index", "Home");
            }

            // 驗證失敗，返回編輯視圖顯示驗證錯誤信息
            return View(customer);
        }
    }


}
