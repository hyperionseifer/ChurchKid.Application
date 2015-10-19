using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Training
{
    public class TrainingClass
    {

        [Key]
        public int TrainingClassId { get; set; }

        [ForeignKey("Training")]
        public int TrainingId { get; set; }

        public virtual Training Training { get; set; }

        [ForeignKey("TrainingLevel")]
        public int TrainingLevelId { get; set; }

        public virtual TrainingLevel TrainingLevel { get; set; }

    }
}
