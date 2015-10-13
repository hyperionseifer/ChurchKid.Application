using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.UserProfile
{
    public class RolePrivilege
    {
        [Key]
        public int RolePrivilegeId { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        [ForeignKey("Privilege")]
        public int PrivilegeId { get; set; }

        public virtual Privilege Privilege { get; set; }
    
    }
}
