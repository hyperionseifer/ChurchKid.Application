using System;
using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities
{
    public class NamedDescriptiveEntity : NamedEntity, IDescriptiveEntity
    {

        public NamedDescriptiveEntity()
        {
            Name = string.Empty;
            Description = string.Empty;
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
        }

        [MaxLength(255)]
        public string Description { get; set; }

    }
}
