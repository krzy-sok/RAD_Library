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
                        _context.Leases.Where(l => l.Active == true).Include(b => b.book).Include(u => u.user).ToList() : [];
        }

        [HttpGet]
        [Route("inactive")]
        [Authorize(Policy = "Librarian")]
        public IEnumerable<Leases> Inactive()
        {
            return _context.Leases != null ?
                        _context.Leases.Where(l => l.Active == false).Include(b => b.book).Include(u => u.user).ToList() : [];
        }


        [HttpGet("details/{id:int}")]
        public IResult Details(int id)
        {

            if (id == null || _context.Leases == null)
            {
                return Results.BadRequest();
            }

            var leases = _context.Leases.Include(b => b.book).Include(u => u.user).FirstOrDefault(l => l.Id == id);
            if (leases == null)
            {
                return Results.NotFound();
            }
            string version = System.Convert.ToBase64String(leases.RowVersion);
            return Results.Json(new { id = leases.Id, leaseStart = leases.leaseStart, leaseEnd = leases.leaseEnd, book = leases.book, user = leases.user, Type = leases.Type, Active = leases.Active, RowVersion = version });
        }

        //[Authorize(Policy = "Librarian")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, Leases leases)
        {
            //Console.WriteLine($"\n*****\n\n object : {leases} \n\n ***** \n");
            Console.WriteLine($"\n*****\n\n version : {leases.RowVersion} \n\n ***** \n");
            Console.WriteLine("\n*****\n\n in lease edit \n\n ***** \n");
            //byte[] RowVersion = System.Convert.FromBase64String(version);
            //byte[] RowVersion = Encoding.ASCII.GetBytes(version);
            if (id != leases.Id)
            {

                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            _context.Entry(leases).Property("RowVersion").OriginalValue = leases.RowVersion;
            Console.WriteLine("\n*****\n\n after rowversion \n\n ***** \n");
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
        [HttpDelete("delete/{id:int}")]
        [Authorize(Policy = "Librarian")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine("\n*****\n\n delete leas \n\n ***** \n");
            if (_context.Leases == null)
            {
                return BadRequest();
            }
            var leases = await _context.Leases.FindAsync(id);

            if (leases != null)
            {
                _context.Leases.Remove(leases);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        [Authorize(Policy = "Librarian")]
        [HttpGet("lease/{id:int}/{version}")]
        public IResult Lease([FromRoute] int id, [FromRoute] string version)
        {
            byte[] RowVersion = System.Convert.FromBase64String(version);
            if (_context.Leases == null)
            {
                //return Problem("Entity set 'ReactLibraryContext.Leases'  is null.");
                return Results.StatusCode(501);
            }
            if (_context.Book == null)
            {
                //return Problem("Entity set 'ReactLibraryContext.Book'  is null.");
                return Results.StatusCode(501);
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
                    //TempData["result"] = $"Changed reservation of {book.Title} to Lease";
                    return Results.Ok();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                    return Results.Conflict();
                }
                return Results.Ok();

            }

            //return RedirectToAction("Index")
            return Results.BadRequest();
        }

        [Authorize(Policy = "Librarian")]
        [HttpGet("return/{id:int}")]
        public IResult Return(int id)
        {
            if (_context.Leases == null)
            {
                //return Problem("Entity set 'ReactLibraryContext.Leases'  is null.");
                return Results.StatusCode(501);
            }
            if (_context.Book == null)
            {
                //return Problem("Entity set 'ReactLibraryContext.Book'  is null.");
                return Results.StatusCode(501);
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
                    //TempData["result"] = $"Removed lease of {book.Title}";
                    _context.SaveChanges();
                    return Results.Ok();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //TempData["error"] = "Concurrency event. Changes not made. Please refresh the page";
                    return Results.Conflict();
                }
            }
            else
            {
                //return Problem("lease id not provided");
                return Results.BadRequest();
            }
        }

        private bool LeasesExists(int id)
        {
          return (_context.Leases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
