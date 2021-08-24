using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Navigation.Data;
using Navigation.Models;
using System.Web;

namespace Navigation.Controllers
{
    [Authorize(Roles = "Driver")]
    public class MyRouteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyRouteController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        private string GetDriverEmail() => HttpContext.User.Identity?.Name;

        private async Task<Driver> GetDriverAsync() =>
            await _context.Drivers.FirstOrDefaultAsync(x => x.Identity.UserName == GetDriverEmail());

        public async Task<IActionResult> Index()
        {
            // Sample Google maps url:
            // https://www.google.com/maps/dir/?api=1&origin=Space+Needle+Seattle+WA&destination=Pike+Place+Market+Seattle+WA&travelmode=bicycling
            
            const string urlStart = @"https://www.google.com/maps/dir/?api=1&destination=";
            const string urlEnd = @"&travelmode=driving";

            var destinations = GetDriverAsync().Result.Destinations;
            
            
            foreach (var destination in destinations)
            {
                destination.Address = urlStart + HttpUtility.UrlEncode(destination.Address) + urlEnd;
            }
            
            destinations.OrderBy(x => x.DueTime);
            
            return View(destinations);
        }
    }
}