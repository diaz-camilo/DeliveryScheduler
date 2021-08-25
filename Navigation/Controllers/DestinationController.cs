using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Navigation.Data;
using Navigation.Models;

namespace Navigation.Controllers
{
    public class DestinationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestinationController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        private Admin GetAdmin()
        {
            // Get logged in Admin
            var adminUserName = HttpContext.User.Identity?.Name;
            var admin = _context.Admin.FirstOrDefault(x => x.Identity.UserName == adminUserName);
            return admin;
        }

        

        // GET: Destination/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .Include(d => d.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        // GET: Destination/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            var drivers = _context.Drivers.Where(x => x.AdminID == GetAdmin().AdminID);
            
            ViewBag.DriverID = new SelectList(drivers, "DriverID", "Name", destination.DriverID);
            return View(destination);
        }

        // POST: Destination/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,Notes,CustomerName,DueTime,CustomerMobile,DriverID")] Destination destination)
        {
            if (id != destination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinationExists(destination.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminController.ListDestinations),"Admin");
            }
            
            ViewBag.DriverID = new SelectList(_context.Drivers, "DriverID", "Name", destination.DriverID);
            return View(destination);
        }

        // GET: Destination/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .Include(d => d.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        // POST: Destination/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminController.ListDestinations),"Admin");
        }
        
        // GET: Destination/DeleteAll/5
        public async Task<IActionResult> DeleteAll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destinations = await _context.Destinations
                .Include(d => d.Driver)
                .Where(m => m.DriverID == id).ToListAsync();
            if (destinations.IsNullOrEmpty())
            {
                return NotFound();
            }

            return View(destinations);
        }

        // POST: Destination/DeleteAll/5
        [HttpPost, ActionName("DeleteAll")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllConfirmed(int id)
        {
            var destinations = await _context.Destinations
                .Where(x => x.DriverID == id).ToListAsync();
            _context.Destinations.RemoveRange(destinations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminController.ListDestinations),"Admin");
        }

        private bool DestinationExists(int id)
        {
            return _context.Destinations.Any(e => e.Id == id);
        }

        private bool isDriverUnderAdmin(int driverId)
        {
            var drivers = GetAdmin().Drivers;

            return true;

        }
        
        // Unused CRUD methods
        
        /*
        // GET: Destination
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Destinations.Include(d => d.Driver);
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: Destination/Create
        public IActionResult Create()
        {
            ViewData["DriverID"] = new SelectList(_context.Drivers, "DriverID", "DriverID");
            return View();
        }

        // POST: Destination/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Notes,CustomerName,DueTime,CustomerMobile,DriverID")] Destination destination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(destination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverID"] = new SelectList(_context.Drivers, "DriverID", "DriverID", destination.DriverID);
            return View(destination);
        }*/
    }
}
