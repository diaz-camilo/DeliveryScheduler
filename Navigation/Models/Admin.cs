using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Navigation.Models
{
    public class Admin
    {
        public int AdminID { get; set; }
        
        public string Name { get; set; }
        
        public string Mobile { get; set; }
        
        public string IdentityID { get; set; }
        public virtual IdentityUser Identity { get; set; }

        public virtual List<Driver> Drivers { get; set; }
        
    }
}