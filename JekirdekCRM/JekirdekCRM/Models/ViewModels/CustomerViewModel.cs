using System.ComponentModel.DataAnnotations;

namespace JekirdekCRM.Models.ViewModels
{
    public class CustomerViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
