using System;

namespace ChurchKid.Data.Entities
{
    public interface INamedPerson
    {

        string FirstName { get; set; }

        string MiddleName { get; set; }

        string LastName { get; set; }

        string NickName { get; set; }
    
    }
}
