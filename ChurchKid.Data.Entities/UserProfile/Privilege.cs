using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities.UserProfile
{
    public class Privilege : IDescriptiveEntity
    {

        public Privilege()
        {
            PrivilegeOption = string.Empty;
            Description = string.Empty;
        }

        [Key]
        public int PrivilegeId { get; set; }

        [MaxLength(100)]
        public string PrivilegeOption { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    
    }
}
