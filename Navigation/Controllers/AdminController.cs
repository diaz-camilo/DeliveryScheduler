using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Navigation.Data;
using Navigation.Models;

namespace Navigation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AdminController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private Admin GetAdmin()
        {
            // Get logged in Admin
            var adminUserName = HttpContext.User.Identity?.Name;
            var admin = _context.Admin.FirstOrDefault(x => x.Identity.UserName == adminUserName);
            return admin;
        }
        
        [HttpGet]
        public IActionResult AddDriver()
        {
            return View();
        }

        // Creates driver login username and password and assign driver to the admin
        // List of drivers.
        [HttpPost]
        public async Task<IActionResult> AddDriver(AddDriverModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser()
            {
                UserName = model.Email,
                Email = model.Email
            };
            
            // Create driver login
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("",error.Description);
                return View(model);
            }

            // Get newly created driver Identity
            var driverIdentity = _userManager.Users
                .FirstOrDefault(x => x.Email == model.Email);

            // Add to driver role
            await _userManager.AddToRoleAsync(driverIdentity, "Driver");
            
            var driver = new Driver()
            {
                Name = model.Name,
                Mobile = model.Mobile,
                IdentityID = driverIdentity.Id,
            };

            // Get logged in Admin userName
            var adminUserName = HttpContext.User.Identity?.Name;
            
            // Add driver to Admin drivers
            _context.Admin.FirstOrDefault(x => x.Identity.UserName == adminUserName)
                ?.Drivers
                .Add(driver);
            await _context.SaveChangesAsync();

            ViewBag.isSuccess = true;
            return View();
        }

        public async Task<IActionResult> ListDrivers()
        {
            var admin = GetAdmin();

            var drivers = admin.Drivers;

            return View(drivers);
        }

        [HttpGet]
        public async Task<IActionResult> AddDestination(int? id = 0)
        {
            var admin = GetAdmin();
            var drivers = admin.Drivers;
            var selectListOfDrivers = 
                drivers.Select(driver => new SelectListItem(driver.Name, driver.DriverID.ToString())).ToList();

            var model = new AddDestinationModel()
            {
                Drivers = selectListOfDrivers
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDestination(AddDestinationModel model)
        {
            var admin = GetAdmin();
            var drivers = admin.Drivers;
            var selectListOfDrivers = 
                drivers.Select(driver => new SelectListItem(driver.Name, driver.DriverID.ToString())).ToList();
            
            if (!ModelState.IsValid)
            {
                model.Drivers = selectListOfDrivers;
                return View(model);
            }

            var driver = drivers.FirstOrDefault(x => x.DriverID == model.DriverID);
            
            _context.Destinations.Add(new Destination()
            {
                Address = model.Address,
                CustomerMobile = model.CustomerMobile,
                CustomerName = model.CustomerName,
                DueTime = model.DueTime,
                Notes = model.Notes,
                Driver = driver
            });
            await _context.SaveChangesAsync();

            ViewBag.isSuccess = true;
            ModelState.Clear();
            var successMode = new AddDestinationModel()
            {
                Drivers = selectListOfDrivers
            };

            return View(successMode);
        }

        public IActionResult ListDestinations(int? i)
        {
            // todo pass selected driver or all drivers
            var admin = GetAdmin();

            var drivers = admin.Drivers;

            return View(drivers);
        }
        
        

        // CRUD methods
        
        /*
        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Admin.Include(u => u.Identity);
            return View(await applicationDbContext.ToListAsync());
        }
        
        //GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Admin
                .Include(u => u.Identity)
                .FirstOrDefaultAsync(m => m.AdminID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewData["IdentityID"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminID,Name,Mobile,IdentityID")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdentityID"] = new SelectList(_context.Users, "Id", "Id", admin.IdentityID);
            return View(admin);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Admin.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["IdentityID"] = new SelectList(_context.Users, "Id", "Id", user.IdentityID);
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminID,Name,Mobile,IdentityID")] Admin admin)
        {
            if (id != admin.AdminID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(admin.AdminID))
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
            ViewData["IdentityID"] = new SelectList(_context.Users, "Id", "Id", admin.IdentityID);
            return View(admin);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Admin
                .Include(u => u.Identity)
                .FirstOrDefaultAsync(m => m.AdminID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Admin.FindAsync(id);
            _context.Admin.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Admin.Any(e => e.AdminID == id);
        }*/
    }
}
