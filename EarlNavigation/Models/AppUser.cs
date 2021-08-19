using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EarlNavigation.Models
{
    public class AppUser : IdentityUser
    {
        public List<Destination> Destinations { get; set; }
    }
}