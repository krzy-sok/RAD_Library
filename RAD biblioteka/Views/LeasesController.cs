﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer.Localisation;
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
                          View(await _context.Leases.Where(l => l.Active == true).Include(b => b.book).Include(u => u.user).ToListAsync()) :
                          Problem("Entity set 'RAD_bibliotekaContext.Leases'  is null.");
        }

        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Inactive()
        {
            return _context.Leases != null ?
                        View(await _context.Leases.Where(l => l.Active == false).Include(b => b.book).Include(u => u.user).ToListAsync()) :
                        Problem("Entity set 'RAD_bibliotekaContext.Leases'  is null.");
        }

        // GET: Leases/Edit/5
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return NotFound();
            }

            var leases =  await _context.Leases.Include(b => b.book).Include(u => u.user).FirstOrDefaultAsync(l => l.Id == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,leaseStart,leaseEnd,Book,User, Type, Active")] Leases leases, byte[] RowVersion)
        {
            //byte[] RowVersion = Encoding.ASCII.GetBytes(version);
            if (id != leases.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            _context.Entry(leases).Property("RowVersion").OriginalValue = RowVersion;
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
                    TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                }
                //}
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
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
                await _context.SaveChangesAsync();
                //_context.Entry(leases).Property("RowVersion").OriginalValue = RowVersion;
                //try 
                //{

                //}
                //catch(DbUpdateConcurrencyException ex)
                //{
                //    TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                //}
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Lease(int? id, string version)
        {
            //byte[] RowVersion = Encoding.ASCII.GetBytes(version);
            byte[] RowVersion = System.Convert.FromBase64String(version);
            if (_context.Leases == null)
            {
                return Problem("Entity set 'RAD_bibliotekaContext.Leases'  is null.");
            }
            if (_context.Book == null)
            {
                return Problem("Entity set 'RAD_bibliotekaContext.Book'  is null.");
            }
            if (id != null)
            {
                Leases lease = _context.Leases.Include(b => b.book).Include(u => u.user).FirstOrDefault(l => l.Id == id);
                Book book = lease.book;
                lease.Active = false;
                lease.leaseEnd = DateTime.Today;
                Leases newLease = new Leases();
                newLease.leaseStart = DateTime.Today;
                newLease.book = book;
                newLease.user = lease.user;
                newLease.Active = true;
                newLease.Type = "Lease";

                book.Status = "Leased";
                _context.Entry(lease).Property("RowVersion").OriginalValue = RowVersion;
                try
                {
                    _context.Leases.Update(lease);
                    _context.Leases.Add(newLease);
                    _context.Book.Update(book);
                    _context.SaveChanges();
                    TempData["result"] = $"Changed reservation of {book.Title} to Lease";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                }

            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Return(int? id)
        {
            if (_context.Leases == null)
            {
                return Problem("Entity set 'RAD_bibliotekaContext.Leases'  is null.");
            }
            if (_context.Book == null)
            {
                return Problem("Entity set 'RAD_bibliotekaContext.Book'  is null.");
            }
            if (id != null)
            {
                Leases lease = _context.Leases.Include(b => b.book).FirstOrDefault(l => l.Id == id);
                Book book = lease.book;
                book.Status = "Available";
                lease.Active = false;
                lease.leaseEnd = DateTime.Today;

                try
                {
                    _context.Leases.Update(lease);
                    _context.Book.Update(book);
                    TempData["result"] = $"Removed lease of {book.Title}";
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                }
            }
            else
            {
                return Problem("lease id not provided");
            }

            return RedirectToAction("Index");
        }

        private bool LeasesExists(int id)
        {
          return (_context.Leases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
