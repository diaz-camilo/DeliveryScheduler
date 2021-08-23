using System;

namespace Navigation.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public string CustomerName { get; set; }
        public DateTime DueTime { get; set; }
        public int StopNumber { get; set; }
        public string CustomerMobile { get; set; }

        public int DriverID { get; set; }
        public virtual Driver Driver { get; set; }
    }
}