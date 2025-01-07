using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReactLibrary.Server.Data;
using ReactLibrary.Server.Models;

namespace ReactLibrary.Server.Views
{
    [ApiController]
    [Route("[controller]")]
    public class LeasesController : ControllerBase
    {
        private readonly ReactLibraryContext _context;

        public LeasesController(ReactLibraryContext context)
        {
            _context = context;
        }

        // GET: Leases
        [HttpGet]
        [Authorize(Policy = "Librarian")]
        public IEnumerable<Leases> Index()
        {
              return _context.Leases != null ? 
                          _context.Leases.Where(l => l.Active == true).Include(b => b.book).Include(u => u.user).ToList() : [] ; 
        }

        [HttpGet]
        [Route("inactive")]
        [Authorize(Policy = "Librarian")]
        public IEnumerable<Leases> Inactive()
        {
            return _context.Leases != null ?
                        _context.Leases.Where(l => l.Active == false).Include(b => b.book).Include(u => u.user).ToList() : [];
        }

        // GET: Leases/Edit/5
        [HttpGet("{id:int}")]
        [Authorize(Policy = "Librarian")]
        public IResult Details(int? id)
        {
            if (id == null || _context.Leases == null)
            {
                return Results.NotFound();
            }

            var leases =  _context.Leases.Include(b => b.book).Include(u => u.user).FirstOrDefault(l => l.Id == id);
            if (leases == null)
            {
                return Results.NotFound();
            }
            return Results.Json(leases);
        }

        // POST: Leases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Librarian")]
        [HttpPut("{id}/{RowVersion}")]
        public async Task<IActionResult> Edit([FromRoute]int id,[FromRoute] byte[] RowVersion, Leases leases)
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
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeasesExists(leases.Id))
                {
                    return NotFound();
                }
                else
                {
                    //TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                    return Conflict();
                }
            }
        }


        // POST: Leases/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leases == null)
            {
                return Problem("Entity set 'ReactLibraryContext.Leases'  is null.");
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

        //public IActionResult Lease(int? id, string version)
        //{
        //    //byte[] RowVersion = Encoding.ASCII.GetBytes(version);
        //    byte[] RowVersion = System.Convert.FromBase64String(version);
        //    if (_context.Leases == null)
        //    {
        //        //return Problem("Entity set 'ReactLibraryContext.Leases'  is null.");
        //        return NotFound();
        //    }
        //    if (_context.Book == null)
        //    {
        //        //return Problem("Entity set 'ReactLibraryContext.Book'  is null.");
        //        return NotFound();
        //    }
        //    if (id != null)
        //    {
        //        Leases lease = _context.Leases.Include(b => b.book).Include(u => u.user).FirstOrDefault(l => l.Id == id);
        //        Book book = lease.book;
        //        lease.Active = false;
        //        lease.leaseEnd = DateTime.Today;
        //        Leases newLease = new Leases();
        //        newLease.leaseStart = DateTime.Today;
        //        newLease.book = book;
        //        newLease.user = lease.user;
        //        newLease.Active = true;
        //        newLease.Type = "Lease";

        //        book.Status = "Leased";
        //        _context.Entry(lease).Property("RowVersion").OriginalValue = RowVersion;
        //        try
        //        {
        //            _context.Leases.Update(lease);
        //            _context.Leases.Add(newLease);
        //            _context.Book.Update(book);
        //            _context.SaveChanges();
        //            //TempData["result"] = $"Changed reservation of {book.Title} to Lease";
        //            return Ok();

        //        }
        //        catch (DbUpdateConcurrencyException ex)
        //        {
        //            //TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
        //            return Conflict();
        //        }

        //    }

        //    //return RedirectToAction("Index")
        //    return BadRequest();
        //}

        //public IActionResult Return(int? id)
        //{
        //    if (_context.Leases == null)
        //    {
        //        //return Problem("Entity set 'ReactLibraryContext.Leases'  is null.");
        //        return BadRequest();
        //    }
        //    if (_context.Book == null)
        //    {
        //        //return Problem("Entity set 'ReactLibraryContext.Book'  is null.");
        //        return BadRequest();
        //    }
        //    if (id != null)
        //    {
        //        Leases lease = _context.Leases.Include(b => b.book).FirstOrDefault(l => l.Id == id);
        //        Book book = lease.book;
        //        book.Status = "Available";
        //        lease.Active = false;
        //        lease.leaseEnd = DateTime.Today;

        //        try
        //        {
        //            _context.Leases.Update(lease);
        //            _context.Book.Update(book);
        //            //TempData["result"] = $"Removed lease of {book.Title}";
        //            _context.SaveChanges();
        //            return Ok();
        //        }
        //        catch (DbUpdateConcurrencyException ex)
        //        {
        //            //TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
        //            return Conflict();
        //        }
        //    }
        //    else
        //    {
        //        //return Problem("lease id not provided");
        //        return NotFound();
        //    }
        //}

        private bool LeasesExists(int id)
        {
          return (_context.Leases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
