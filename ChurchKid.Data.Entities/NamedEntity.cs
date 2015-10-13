using System;
using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities
{
    public class NamedEntity : INamedEntity, ICreatedEntity, ITimestampedEntity
    {

        public NamedEntity()
        {
            Name = string.Empty;
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
        }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime LastModified { get; set; }
    
    }
}
