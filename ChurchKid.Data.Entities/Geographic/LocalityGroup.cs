using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Geographic
{
    public class LocalityGroup : NamedDescriptiveEntity
    {

        [Key]
        public int LocalityGroupId { get; set; }

        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
    
    }
}
