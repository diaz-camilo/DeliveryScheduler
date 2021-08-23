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
        
        public string GetUserEmail() => HttpContext.User.Identity.Name;

        public async Task<IActionResult> Index()
        {
            const string urlStart = @"https://www.google.com/maps/dir/?api=1&destination=";
            const string urlEnd = @"&travelmode=driving";
            
                
            
            // var userEmail = GetUserEmail();
            //
            // var user = await _context.Admin.FirstOrDefaultAsync(x => x.Identity.Email == userEmail);
            //
            // var destinations = _context.Destinations.Where(x => x.Driver == user);
            //
            // // https://www.google.com/maps/dir/?api=1&origin=Space+Needle+Seattle+WA&destination=Pike+Place+Market+Seattle+WA&travelmode=bicycling
            // foreach (var destination in destinations)
            // {
            //     destination.Address = urlStart + HttpUtility.UrlEncode(destination.Address) + urlEnd;
            // }
            //
            // destinations.OrderBy(x => x.DueTime);
            //
            // return View(destinations);
            return View();
        }
    }
}