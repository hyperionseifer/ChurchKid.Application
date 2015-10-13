using System;

namespace ChurchKid.Data.Entities
{
    public interface ITimestampedEntity
    {

        public DateTime LastModified { get; set; }
    
    }
}
