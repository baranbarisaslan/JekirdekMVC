using System.ComponentModel.DataAnnotations;

namespace JekirdekCRM.Models.DBModels
{
    public class Customer : EntityBase
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

        public bool isDeleted { get; set; }
    }
}
