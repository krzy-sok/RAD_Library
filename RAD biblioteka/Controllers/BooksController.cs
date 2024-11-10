using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAD_biblioteka.Data;
using RAD_biblioteka.Models;
using Microsoft.AspNetCore.Authorization;

namespace RAD_biblioteka.Controllers
{
    public class BooksController : Controller
    {
        private readonly RAD_bibliotekaContext _context;

        public BooksController(RAD_bibliotekaContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string bookStatus, string searchString)
        {
            IQueryable<string> stausQuery = from b in _context.Book orderby b.Status select b.Status;
            var books = from b in _context.Book select b;

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
              //            Problem("Entity set 'RAD_bibliotekaContext.Book'  is null.");
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
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Publisher,PublicationDate,Price,Status")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Publisher,PublicationDate,Price,Status")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            }
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
                return Problem("Entity set 'RAD_bibliotekaContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
