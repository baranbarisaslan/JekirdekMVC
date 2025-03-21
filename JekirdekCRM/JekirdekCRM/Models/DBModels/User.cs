using System.ComponentModel.DataAnnotations;

namespace JekirdekCRM.Models.DBModels
{
    public class User :EntityBase
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [MaxLength(36)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
