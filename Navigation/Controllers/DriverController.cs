using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Navigation.Data;
using Navigation.Models;

namespace Navigation.Controllers
{
    public class DriverController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DriverController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        

        // GET: Driver/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.Admin)
                .Include(d => d.Identity)
                .FirstOrDefaultAsync(m => m.DriverID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        

        // GET: Driver/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(x => x.Identity)
                .FirstOrDefaultAsync(x => x.DriverID == id);
            if (driver == null)
            {
                return NotFound();
            }

            var driverModel = new EditDriverModel()
            {
                DriverID = driver.DriverID,
                Name = driver.Name,
                Mobile = driver.Mobile,
                Email = driver.Identity.Email
            };
            
            return View(driverModel);
        }

        // POST: Driver/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DriverID,Name,Mobile,Email")] EditDriverModel model)
        {
            if (id != model.DriverID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var driver = await _context.Drivers
                    .Include(x => x.Identity)
                    .FirstOrDefaultAsync(x => x.DriverID == model.DriverID);

                if (driver != null)
                {
                    driver.Name = model.Name;
                    driver.Mobile = model.Mobile;
                }
                else
                {
                    return NotFound();
                }
                
                var user = await _userManager.FindByIdAsync(driver.IdentityID);
                user.UserName = model.Email;
                user.Email = model.Email;
                try
                {
                    
                    await _context.SaveChangesAsync();
                    await _userManager.UpdateAsync(user);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(model.DriverID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminController.ListDrivers),"Admin");
            }
            return View(model);
        }

        // GET: Driver/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.Admin)
                .Include(d => d.Identity)
                .FirstOrDefaultAsync(m => m.DriverID == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Driver/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminController.ListDrivers),"Admin");
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.DriverID == id);
        }
        
        // Unused CRUD methods
        
        /*
        // GET: Driver
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Drivers.Include(d => d.Admin).Include(d => d.Identity);
            return View(await applicationDbContext.ToListAsync());
        }
        
        // GET: Driver/Create
        public IActionResult Create()
        {
            ViewData["AdminID"] = new SelectList(_context.Admin, "AdminID", "AdminID");
            ViewData["IdentityID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Driver/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverID,Name,Mobile,IdentityID,AdminID")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminID"] = new SelectList(_context.Admin, "AdminID", "AdminID", driver.AdminID);
            ViewData["IdentityID"] = new SelectList(_context.Users, "Id", "Id", driver.IdentityID);
            return View(driver);
        }*/
    }
}
