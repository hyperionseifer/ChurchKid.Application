using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Geographic
{
    public class Cluster : NamedDescriptiveEntity
    {

        [Key]
        public int ClusterId { get; set; }

        [ForeignKey("Locality")]
        public int LocalityId { get; set; }

        public virtual Locality Locality { get; set; }
    
    }
}
