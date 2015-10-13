using System;
using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities.UserProfile
{
    public class ApplicationUser : NamedPerson
    {

        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }
    
    }

}
