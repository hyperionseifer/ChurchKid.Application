using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities.Geographic
{
    public class Country : NamedEntity
    {

        [Key]
        public int CountryId { get; set; }
    
    }
}
