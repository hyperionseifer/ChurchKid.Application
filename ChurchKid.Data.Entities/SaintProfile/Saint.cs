using ChurchKid.Data.Entities.Geographic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.SaintProfile
{
    public class Saint : NamedPerson
    {

        [Key]
        public int SaintId { get; set; }

        [Required, MaxLength(10)]
        public string Gender { get; set; }

        public int UserId { get; set; }
    
        public int BirthMonth { get; set; }

        public int BirthDay { get; set; }

        public int BirthYear { get; set; }

        [NotMapped]
        public DateTime BirthDate
        {
            get
            {
                var month = BirthMonth;
                var day = BirthDay;
                var year = BirthYear;

                if (month <= 0)
                    month = DateTime.MinValue.Month;

                if (day <= 0)
                    day = DateTime.MinValue.Day;

                if (year <= 0)
                    year = DateTime.MinValue.Year;

                return new DateTime(year, month, day);
            }
        }

        [MaxLength(150)]
        public string BirthPlace { get; set; }

        [MaxLength(10)]
        public int BldgNo { get; set; }
        
        [MaxLength(10)]
        public string HouseNo { get; set; }

        [MaxLength(100)]
        public string Bldg { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }

        [MaxLength(100)]
        public string Subdivision { get; set; }

        [MaxLength(100)]
        public string District { get; set; }

        [MaxLength(100)]
        public int City { get; set; }

        [MaxLength(100)]
        public int Region { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [MaxLength(50), Phone]
        public string PhoneNo { get; set; }

        [MaxLength(50), Phone]
        public string MobileNo { get; set; }

        [MaxLength(50), Phone]
        public string Fax { get; set; }

        [MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [MaxLength(30)]
        public string MaritalStatus { get; set; }

        [MaxLength(100)]
        public string FatherFirstName { get; set; }

        [MaxLength(100)]
        public string FatherMiddleName { get; set; }

        [MaxLength(100)]
        public string FatherLastName { get; set; }

        [MaxLength(100)]
        public string MotherFirstName { get; set; }

        [MaxLength(100)]
        public string MotherMiddleName { get; set; }

        [MaxLength(100)]
        public string MotherLastName { get; set; }

        public string SpouseFirstName { get; set; }

        public string SpouseMiddleName { get; set; }

        public string SpouseLastName { get; set; }

        public int BaptismalMonth { get; set; }

        public int BaptismalDay { get; set; }

        public int BaptismalYear { get; set; }

        [NotMapped]
        public DateTime BaptismalDate
        {
            get
            {
                var month = BaptismalMonth;
                var day = BaptismalDay;
                var year = BaptismalYear;

                if (month <= 0)
                    month = DateTime.MinValue.Month;

                if (day <= 0)
                    day = DateTime.MinValue.Day;

                if (year <= 0)
                    year = DateTime.MinValue.Year;

                return new DateTime(year, month, day);
            }
        }

        [Required, MaxLength(150)]
        public string BaptismalPlace { get; set; }

        [ForeignKey("District")]
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
    
    }
}
