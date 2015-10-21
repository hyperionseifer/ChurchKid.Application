using ChurchKid.Common;
using ChurchKid.Data.Entities.UserProfile;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Audit
{
    public class AuditLog
    {

        public AuditLog()
        {
            DateAndTime = DateTime.Now;
            Details = string.Empty;
            Action = UserActions.Add;
            PerformedAt = string.Empty;
        }

        [Key]
        public int AuditLogId { get; set; }

        [ForeignKey("ApplicationModule")]
        public int ApplicationModuleId { get; set; }

        public virtual ApplicationModule ApplicationModule { get; set; }

        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime DateAndTime { get; set; }

        [Required, MaxLength(20)]
        public string Action { get; set; }

        public int ReferenceId { get; set; }

        [Required]
        public string Details { get; set; }

        [MaxLength(100)]
        public string PerformedAt { get; set; }
    
    }
}
