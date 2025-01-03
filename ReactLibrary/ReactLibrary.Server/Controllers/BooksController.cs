using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReactLibrary.Server.Data;
using ReactLibrary.Server.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
//using ReactLibrary.Server.Views;
using System.Security.AccessControl;

namespace ReactLibrary.Server.Controllers
{
    public class BooksController : Controller
    {
        private readonly ReactLibraryContext _context;

        public BooksController(ReactLibraryContext context)
        {
            _context = context;
        }

        public void CheckExpiery()
        {
            var expired = _context.Leases.Where(l => l.Active == true && l.leaseEnd < DateTime.Today && l.Type == "Reservation").Include(b => b.book).ToList();
            foreach(Leases lease in expired)
            {
                Book book = lease.book;
                book.Status = "Available";
                _context.Update(book);
                lease.Active = false;
                _context.Update(lease);
            }
            _context.SaveChanges();
        }
        // GET: Books
        public async Task<IActionResult> Index(string bookStatus, string searchString)
        {
            CheckExpiery();
            IQueryable<string> stausQuery = from b in _context.Book orderby b.Status select b.Status;
            
            var books = from b in _context.Book select b;
            if (!User.IsInRole("Admin"))
            {
                books = books.Where(b => b.Hidden == false);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                var authors = from b in _context.Book select b;
                books = books.Where(s => s.Title!.Contains(searchString));
                authors = authors.Where(s => s.Author!.Contains(searchString));
                books = books.Concat(authors).Distinct();
            }
            if (!string.IsNullOrEmpty(bookStatus))
            {
                books = books.Where(x => x.Status == bookStatus);
            }
            var bookStatusVM = new BookStausViewModel
            {
                Statuses = new SelectList(await stausQuery.Distinct().ToListAsync()),
                Books = await books.ToListAsync()
            };

            //return View(await books.ToArrayAsync());

            return View(bookStatusVM);
              //return _context.Book != null ? 
              //            View(await _context.Book.ToListAsync()) :
              //            Problem("Entity set 'ReactLibraryContext.Book'  is null.");
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Policy = "Librarian")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Create(Book book)
        {
            book.Hidden = false;
            //book.PublicationDate = DateTime.Parse(book.PublicationDate);
            Console.WriteLine(book.PublicationDate);
            book.Status = "Available";
            //if (ModelState.IsValid)
            //{
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            //else
            //{
            //    Console.WriteLine("****************\n invalid\n");
            //}
            return View(book);
        }

        // GET: Books/Edit/
         [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                //Book book2 = _context.Book.Find(id);

                //book.Status = book.Status;
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ReactLibraryContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book.Status != "Available")
            {
                TempData["error"] = $"Book {book.Title} cannot be deleted now";
                return RedirectToAction(nameof(Index));
            }
            if (book != null)
            {
                if (_context.Leases.Where(l => l.book == book).ToList().Count() != 0)
                {
                    book.Hidden = true;
                    _context.Book.Update(book);
                    TempData["result"] = $"Book {book.Title} has been hidden";
                }
                else
                {
                    _context.Book.Remove(book);
                    TempData["result"] = $"Book {book.Title} has been deleted";
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Reserve(int? id)
        {
            var email = User.Claims.Where(c=>c.Type == ClaimTypes.Email).FirstOrDefault().Value;
            //Console.WriteLine("******************\n");
            //Console.WriteLine(email);
            //Console.WriteLine("\n******************\n");

            Console.WriteLine("******************\n");
            Console.WriteLine(id);
            Console.WriteLine("\n******************\n");

            if(id != null && email != null)
            {
                var book = await _context.Book.FindAsync(id);
                if (book.Status != "Available")
                { 
                    TempData["error"] = $"Book {book.Title} is alredy reserved";
                    return RedirectToAction("Details", new { id = id });
                }
                book.Status = "Reserved";
                _context.Update(book);

                var user = _context.User.Where(x => (x.email == email)).FirstOrDefault();

                var lease = new Leases();
                lease.leaseStart = DateTime.Today;
                lease.leaseEnd = DateTime.Today.AddDays(1);
                lease.book = book;
                lease.user = user;
                lease.Type = "Reservation";
                lease.Active = true;
                _context.Add(lease);

                _context.SaveChanges();

                TempData["result"] = $"Book {book.Title} has been reserved";
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}
