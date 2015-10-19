using ChurchKid.Data.Entities.Miscellaneous;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.SaintProfile
{
    public class SaintEducation
    {

        [Key]
        public int SaintEducationId { get; set; }

        [ForeignKey("Saint")]
        public int SaintId { get; set; }

        public virtual Saint Saint { get; set; }

        [ForeignKey("EducationalLevel")]
        public int EducationalLevelId { get; set; }

        public virtual EducationalLevel EducationalLevel { get; set; }

        [Required, MaxLength(255)]
        public string School { get; set; }

        [MaxLength(150)]
        public string Course { get; set; }

        [MaxLength(150)]
        public string Major { get; set; }

        public int AttendedMonth { get; set; }

        public int AttendedDay { get; set; }

        public int AttendedYear { get; set; }

        [NotMapped]
        public DateTime AttendedDate
        {
            get
            {
                var month = AttendedMonth;
                var day = AttendedDay;
                var year = AttendedYear;

                if (month <= 0)
                    month = DateTime.MinValue.Month;

                if (day <= 0)
                    day = DateTime.MinValue.Day;

                if (year <= 0)
                    year = DateTime.MinValue.Year;

                return new DateTime(year, month, day);
            }
        }

        public int GraduatedMonth { get; set; }

        public int GraduatedDay { get; set; }

        public int GraduatedYear { get; set; }

        [NotMapped]
        public DateTime GraduatedDate
        {
            get
            {
                var month = GraduatedMonth;
                var day = GraduatedDay;
                var year = GraduatedYear;

                if (month <= 0)
                    month = DateTime.MinValue.Month;

                if (day <= 0)
                    day = DateTime.MinValue.Day;

                if (year <= 0)
                    year = DateTime.MinValue.Year;

                return new DateTime(year, month, day);
            }
        }

    }
}
