using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Navigation.Models;

namespace Navigation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Navigation.Models.Destination> Destinations { get; set; }
        public DbSet<Navigation.Models.Admin> Admin { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}