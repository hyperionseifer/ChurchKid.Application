using System;

namespace ChurchKid.Data.Entities
{
    public interface INamedEntity
    {

        public int Id { get; set; }

        public string Name { get; set; }
    
    }
}
