using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Training
{
    public class Training : NamedEntity
    {

        [Key]
        public int TrainingId { get; set; }

        [ForeignKey("TrainingCenter")]
        public int TraininCenterId { get; set; }

        public virtual TrainingCenter TrainingCenter { get; set; }

        public int Term { get; set; }
        
        public DateTime ProjectedStartDate { get; set; }

        public DateTime ProjectedEndDate { get; set; }

        public DateTime ActualStartDate { get; set; }

        public DateTime ActualEndDate { get; set; }

        [Required, MaxLength(15)]
        public string Status { get; set; }

    }
}
