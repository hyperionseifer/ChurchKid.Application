using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities.UserProfile
{
    public class Privilege : IDescriptiveEntity
    {

        public Privilege()
        {
            Key = string.Empty;
            Description = string.Empty;
        }

        [Key]
        public int PrivilegeId { get; set; }

        [Required, MaxLength(50)]
        public string Key { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    
    }
}
