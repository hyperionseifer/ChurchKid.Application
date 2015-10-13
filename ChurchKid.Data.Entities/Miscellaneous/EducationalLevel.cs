using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities.Miscellaneous
{
    public class EducationalLevel : NamedDescriptiveEntity
    {

        [Key]
        public int EducationalLevelId { get; set; }
    }

}
