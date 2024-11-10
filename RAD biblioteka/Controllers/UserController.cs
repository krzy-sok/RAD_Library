using Microsoft.AspNetCore.Mvc;
using RAD_biblioteka.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using RAD_biblioteka.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace RAD_biblioteka.Controllers
{
    public class UserController : Controller
    {
        private readonly RAD_bibliotekaContext _context;

        public UserController(RAD_bibliotekaContext context)
        {
            _context = context;
        }
        public string HashPasswd(string passwd)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(passwd));

            return Encoding.UTF8.GetString(hashValue);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.firstName = model.firstName;
                user.email = model.email;  
                user.lastName = model.lastName;
                user.phoneNumber = model.phoneNumber;
                user.userName = model.userName;
                user.password = HashPasswd(model.password);
                user.admin = false;

                try
                {
                    _context.Add(user);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{user.userName} registerd succesfully";
                }
                catch(DbUpdateException ex)
                {
                    ModelState.AddModelError("", "User with that email already exists!");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hash = HashPasswd(model.password);
                var user = _context.User.Where(x => (x.userName == model.userNameOrEmail || x.email == model.userNameOrEmail) && x.password == hash).FirstOrDefault();
                if(user != null)
                {
                    //success add cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.email),
                        new Claim("Name", user.firstName),
                        new Claim(ClaimTypes.Role, "User")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login data");
                }
            }
            return View(model);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult LandingPage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
