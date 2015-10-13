using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Training
{
    public class TrainingAttendee
    {

        [Key]
        public int TrainingAttendeeId { get; set; }

        [ForeignKey("Training")]
        public int TrainingId { get; set; }

        public virtual Training Training { get; set; }

        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }

        public virtual Trainee Trainee { get; set; }

        public DateTime JoinDate { get; set; }

        [Required, MaxLength(15)]
        public string Status { get; set; }

        [MaxLength(255)]
        public string Remarks { get; set; }

        public DateTime LeaveDate { get; set; }

        public DateTime CompletionDate { get; set; }
    
    }
}
