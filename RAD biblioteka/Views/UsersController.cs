using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAD_biblioteka.Data;
using RAD_biblioteka.Models;
using System.Security.Cryptography;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace RAD_biblioteka.Views
{
    public class UsersController : Controller
    {
        private readonly RAD_bibliotekaContext _context;

        public UsersController(RAD_bibliotekaContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Librarian")]
        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'RAD_bibliotekaContext.User'  is null.");
        }


        [Authorize(Policy = "Librarian")]
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}
