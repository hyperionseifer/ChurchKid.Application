using ChurchKid.Data.Entities.Geographic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Training
{
    public class TrainingCenter : NamedEntity
    {

        [Key]
        public int TrainingCenterId { get; set; }

        [ForeignKey("Locality")]
        public int LocalityId { get; set; }

        public virtual Locality Locality { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(75), Phone]
        public string ContactNo { get; set; }

        [MaxLength(120), EmailAddress]
        public string Email { get; set; }
    
    }
}
