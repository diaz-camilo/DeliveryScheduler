using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Navigation.Models
{
    public class AddDestinationModel
    {
        
        [Required]
        public string Address { get; set; }
        public string Notes { get; set; }
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        [Display(Name = "Due Time")]
        public DateTime DueTime { get; set; }
        [Display(Name = "Customer Mobile")]
        public string CustomerMobile { get; set; }
        [Required]
        public int DriverID { get; set; }
        public List<SelectListItem> Drivers { get; set; }
    }
}