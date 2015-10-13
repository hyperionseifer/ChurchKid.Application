using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Geographic
{
    public class Locality : NamedDescriptiveEntity
    {

        [Key]
        public int LocalityId { get; set; }

        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public int LocalityGroupId { get; set; }
    
    }

}
