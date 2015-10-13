using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities
{
    public class NamedPerson : INamedPerson, ICreatedEntity, ITimestampedEntity
    {

        public NamedPerson()
        {
            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
        }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string MiddleName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string NickName { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                var firstName = string.Empty;
                var nickName = string.Empty;
                var lastName = string.Empty;
                
                if (!string.IsNullOrEmpty(FirstName))
                    firstName = FirstName.Trim();

                if (!string.IsNullOrEmpty(NickName))
                    nickName = NickName.Trim();

                if (!string.IsNullOrEmpty(LastName))
                    lastName = LastName.Trim();

                return (!string.IsNullOrEmpty(nickName)? nickName :  firstName) + (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(nickName) ? " " : string.Empty) + lastName;
            }
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                var firstName = string.Empty;
                var lastName = string.Empty;
                var middleName = string.Empty;

                if (!string.IsNullOrEmpty(FirstName))
                    firstName = FirstName.Trim();

                if (!string.IsNullOrEmpty(MiddleName))
                    middleName = MiddleName.Trim();

                if (!string.IsNullOrEmpty(LastName))
                    lastName = LastName.Trim();

                return firstName + (!string.IsNullOrEmpty(firstName) ? " " : string.Empty) + middleName + (!string.IsNullOrEmpty(middleName) ? " " : string.Empty) + lastName;
            }
        }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime LastModified { get; set; }

    }
}
