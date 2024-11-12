using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAD_biblioteka.Data;
using RAD_biblioteka.Models;

namespace RAD_biblioteka.Views
{
    public class LeasesController : Controller
    {
        private readonly RAD_bibliotekaContext _context;

        public LeasesController(RAD_bibliotekaContext context)
        {
            _context = context;
        }

        // GET: Leases
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Index()
        {
              return _context.Leases != null ? 
                          View(await _context.Leases.ToListAsync()) :
                          Problem("Entity set 'RAD_bibliotekaContext.Leases'  is null.");
        }

        // GET: Leases/Details/5
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var leases = await _context.Leases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leases == null)
            {
                return NotFound();
            }

            return View(leases);
        }

        // GET: Leases/Create
        //[Authorize(Policy = "Librarian")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Leases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Policy = "Librarian")]
        //public async Task<IActionResult> Create([Bind("Id,leaseStart,leaseEnd,book,user")] Leases leases)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(leases);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(leases);
        //}

        // GET: Leases/Edit/5
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var leases = await _context.Leases.FindAsync(id);
            if (leases == null)
            {
                return NotFound();
            }
            return View(leases);
        }

        // POST: Leases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,leaseStart,leaseEnd,BookId,UserId")] Leases leases)
        {
            if (id != leases.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leases);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeasesExists(leases.Id))
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
            return View(leases);
        }

        // GET: Leases/Delete/5
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var leases = await _context.Leases
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leases == null)
            {
                return NotFound();
            }

            return View(leases);
        }

        // POST: Leases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leases == null)
            {
                return Problem("Entity set 'RAD_bibliotekaContext.Leases'  is null.");
            }
            var leases = await _context.Leases.FindAsync(id);
            if (leases != null)
            {
                _context.Leases.Remove(leases);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeasesExists(int id)
        {
          return (_context.Leases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
