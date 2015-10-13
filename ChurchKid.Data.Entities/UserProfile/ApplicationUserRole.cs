using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.UserProfile
{
    public class ApplicationUserRole
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    
    }
}
