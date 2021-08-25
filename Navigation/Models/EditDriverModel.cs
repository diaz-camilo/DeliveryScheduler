using System.ComponentModel.DataAnnotations;

namespace Navigation.Models
{
    public class EditDriverModel : Driver
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}