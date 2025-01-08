using Microsoft.AspNetCore.Mvc;
using ReactLibrary.Server.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ReactLibrary.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Data;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ReactLibrary.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ReactLibraryContext _context;

        public UserController(ReactLibraryContext context)
        {
            _context = context;
        }

        public void CheckExpiery()
        {
            var expired = _context.Leases.Where(l => l.Active == true && l.leaseEnd < DateTime.Today && l.Type == "Reservation").Include(b => b.book).ToList();
            foreach (Leases lease in expired)
            {
                Book book = lease.book;
                book.Status = "Available";
                _context.Update(book);
                lease.Active = false;
                _context.Update(lease);
            }
            _context.SaveChanges();
        }
        public string HashPasswd(string passwd)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(passwd));

            return Encoding.UTF8.GetString(hashValue);
        }

        [Authorize]
        [HttpGet]
        [Route("details")]
        public IResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Index", "Home");
                return Results.Forbid();
            }
            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value;
            var user = _context.User.Where(x => (x.email == email)).FirstOrDefault();
            return Results.Json(user);
        }
        [HttpGet]
        [Route("test")]
        public IActionResult test() {
            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Registration(RegistrationViewModel model)
        {
            Console.WriteLine("\n*********\n\n IN REGIOSTER \n\n **********\n");
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
                    return Ok();
                }
                catch (DbUpdateException ex)
                {
                    return StatusCode(406);
                }
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("login")]
        public IResult Login(LoginViewModel model)
        {
            Console.WriteLine("\n********\n\n in login \n\n ******\n");
            Console.WriteLine(model);
            if (ModelState.IsValid)
            {
                string hash = HashPasswd(model.password);
                var user = _context.User.Where(x => (x.userName == model.userNameOrEmail || x.email == model.userNameOrEmail) && x.password == hash).FirstOrDefault();
                if (user != null)
                {
                    //success add cooki
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.email),
                        new Claim(ClaimTypes.Name, user.firstName),
                        new Claim(ClaimTypes.GivenName, user.userName),
                        new Claim(ClaimTypes.Surname, user.lastName),
                        //new Claim(ClaimTypes.MobilePhone, user.phoneNumber)
                        new Claim("id", user.Id.ToString())
                        //new Claim(ClaimTypes.Role, "User")
                    };
                    if (user.admin == true)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    }
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return Results.Json(new { username = user.userName, isadmin = user.admin });
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login data");
                    return Results.StatusCode(406);
                }
            }
            return Results.StatusCode(401);
        }

        [HttpGet]
        [Route("logout")]
        [Authorize]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
            //return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("info")]
        [Authorize]
        public IResult UserInfo()
        {
            Console.WriteLine("\n******\n\n in user info \n\n *******\n");
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
            Console.WriteLine($"\n******\n\n {username} \n\n *******\n");
            return username != null ? Results.Json(new { username = username, role = role }) : Results.Json(new { username = "none" });
        }

        //POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(DeleteViewModel model)
        {
            string hash = HashPasswd(model.password);
            var user = _context.User.Where(x => (x.email == model.userEmail) && x.password == hash).FirstOrDefault();
            CheckExpiery();
            var UserLeases = await _context.Leases.Where(l => l.user == user && l.Active == true).ToListAsync();

            if (UserLeases.Count != 0)
            {
                //ModelState.AddModelError("", "Cannot delete user wieth reservations/Leases");
                return Forbid();
            }
            if (user != null)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
                //return RedirectToAction("Index", "Home");
            }
            //ModelState.AddModelError("", "Incorect user credentials");
            return Unauthorized();
        }

        // GET: Leases
        [HttpGet]
        [Route("leases")]
        public IResult UserLeases()
        {
            CheckExpiery();
            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value;
            var user = _context.User.Where(x => (x.email == email)).FirstOrDefault();
            var UserLeases =  _context.Leases.Where(l => l.user == user && l.Active == true).Include(b => b.book).ToList();
            Console.WriteLine($"**************\n in user leases \n *****************");
            return _context.Leases != null ?
                        Results.Json(UserLeases):
                        Results.Problem("Entity set 'ReactLibraryContext.Leases'  is null.");
        }


        [HttpGet("unlease/{id}/{version}")]
        public async Task<IActionResult> Unlease([FromRoute] int id, [FromRoute] string version)
        {
            byte[] rowversion = System.Convert.FromBase64String(version);
            if (id != null)
            {
                Leases lease = _context.Leases.Where(l => l.Id == id).Include(b => b.book).FirstOrDefault();
                lease.Active = false;

                Book book = lease.book;

                if (book != null)
                {
                    //_context.Entry(lease).Property("RowVersion").OriginalValue = rowversion;
                    book.Status = "Available";
                    try
                    {
                        _context.Leases.Update(lease);
                        _context.Book.Update(book);
                        _context.SaveChanges();
                        //TempData["result"] = $"Removed reservation of {book.Title}";
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        //TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                        return StatusCode(409);
                    }
                }
                //ModelState.AddModelError("", "Book not found");
                return NotFound();
            }
            else
            {
                //ModelState.AddModelError("", "Lease Id not provided");
                return BadRequest();
            }
        }

    }
}
