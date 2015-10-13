using System.ComponentModel.DataAnnotations;
namespace ChurchKid.Data.Entities.UserProfile
{
    public class Role : NamedEntity
    {

        public Role()
        {
            AllowDelete = true;
            AdministrativeRole = false;
        }

        [Key]
        public int RoleId { get; set; }

        public bool AllowDelete { get; set; }

        public bool AdministrativeRole { get; set; }
    
    }
}
