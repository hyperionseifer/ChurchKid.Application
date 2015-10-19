using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Training
{
    public class TrainingAttendee
    {

        [Key]
        public int TrainingAttendeeId { get; set; }

        [ForeignKey("TrainingClass")]
        public int TrainingClassId { get; set; }

        public virtual TrainingClass TrainingClass { get; set; }

        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }

        public virtual Trainee Trainee { get; set; }

        public DateTime JoinDate { get; set; }

        [Required, MaxLength(15)]
        public string Status { get; set; }

        public string Remarks { get; set; }

        public DateTime LeaveDate { get; set; }

        public DateTime CompletionDate { get; set; }
    
    }
}
