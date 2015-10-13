using ChurchKid.Data.Entities.SaintProfile;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Training
{
    public class Trainee
    {

        [Key]
        public int TraineeId { get; set; }

        [ForeignKey("Saint")]
        public int SaintId { get; set; }

        public virtual Saint Saint { get; set; }
    
    }
}
