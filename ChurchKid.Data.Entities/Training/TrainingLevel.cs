using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities.Training
{
    public class TrainingLevel : NamedDescriptiveEntity
    {
        [Key]
        public int TrainingLevelId { get; set; }

    }
}
