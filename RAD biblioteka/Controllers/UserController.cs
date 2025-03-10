﻿using Microsoft.AspNetCore.Mvc;
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
using System.Data;
using System.Xml.Serialization;

namespace RAD_biblioteka.Controllers
{
    public class UserController : Controller
    {
        private readonly RAD_bibliotekaContext _context;

        public UserController(RAD_bibliotekaContext context)
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
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
                catch (DbUpdateException ex)
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
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Delete()
        {
            return View();
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
                ModelState.AddModelError("", "Cannot delete user wieth reservations/Leases");
                return View(model);
            }
            if (user != null)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Incorect user credentials");
            return View(model);
        }

        // GET: Leases
        public async Task<IActionResult> UserLeases()
        {
            CheckExpiery();
            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value;
            var user = _context.User.Where(x => (x.email == email)).FirstOrDefault();
            var UserLeases = await _context.Leases.Where(l => l.user == user && l.Active == true).Include(b => b.book).ToListAsync();
            Console.WriteLine($"**************\n in user leases \n *****************");
            return _context.Leases != null ?
                        View(UserLeases) :
                        Problem("Entity set 'RAD_bibliotekaContext.Leases'  is null.");
        }

        public async Task<IActionResult> Unlease(int id, string version)
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
                        TempData["result"] = $"Removed reservation of {book.Title}";
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                    }
                }
                ModelState.AddModelError("", "Book not found");
            }
            else
            {
                ModelState.AddModelError("", "Lease Id not provided");
            }

            return RedirectToAction("UserLeases");
        }

    }
}
