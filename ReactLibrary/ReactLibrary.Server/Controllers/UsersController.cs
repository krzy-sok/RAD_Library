using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReactLibrary.Server.Data;
using ReactLibrary.Server.Models;
using System.Security.Cryptography;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ReactLibrary.Server.Views
{
    public class UsersController : Controller
    {
        private readonly ReactLibraryContext _context;

        public UsersController(ReactLibraryContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Librarian")]
        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'ReactLibraryContext.User'  is null.");
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
