using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Navigation.Models
{
    public class Destination
    {
        public int Id { get; set; }
        
        [Required]
        public string CustomerName { get; set; }
        
        [Required]
        public string Address { get; set; }
        
        public string CustomerMobile { get; set; }
        
        public string Notes { get; set; }
        [Required]
        public DateTime DueTime { get; set; }
       

        public int DriverID { get; set; }
        public virtual Driver Driver { get; set; }
    }
}
