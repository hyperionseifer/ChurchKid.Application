using System;

namespace ChurchKid.Data.Entities
{
    public interface ICreatedEntity
    {

        DateTime DateCreated { get; set; }

        int CreatedById { get; set; }
    
    }
}
