using System;

namespace ChurchKid.Data.Entities
{
    public interface INamedPerson
    {

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }
    
    }
}
