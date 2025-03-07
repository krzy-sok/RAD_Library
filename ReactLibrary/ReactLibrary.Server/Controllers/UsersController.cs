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
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ReactLibraryContext _context;

        public UsersController(ReactLibraryContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Librarian")]
        [HttpGet]
        public IEnumerable<User> Index()
        {
              return _context.User != null ? 
                           _context.User.ToList() : [];
        }


        [Authorize(Policy = "Librarian")]
        [HttpGet("{id:int}")]
        public IResult Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return Results.NotFound();
            }

            var user = _context.User
                .FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return Results.NotFound();
            }

            return Results.Json(user);
        }
    }
}
