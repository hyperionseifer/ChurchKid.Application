using System;

namespace ChurchKid.Data.Entities
{
    public interface ITimestampedEntity
    {

        DateTime LastModified { get; set; }
    
    }
}
