using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Navigation.Models
{
    public class Driver
    {
        public int DriverID { get; set; }
        
        public string Name { get; set; }
        
        public string Mobile { get; set; }
        
        public string IdentityID { get; set; }
        public virtual IdentityUser Identity { get; set; }

        public int AdminID { get; set; }
        public virtual Admin Admin { get; set; }

        public virtual List<Destination> Destinations { get; set; }
        
        
    }
}