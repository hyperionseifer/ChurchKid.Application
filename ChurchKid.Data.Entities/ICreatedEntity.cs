using System;

namespace ChurchKid.Data.Entities
{
    public interface ICreatedEntity
    {

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }
    
    }
}
