using System.ComponentModel.DataAnnotations;

namespace ChurchKid.Data.Entities.Audit
{
    public class ApplicationModuleGroup : NamedEntity
    {

        [Key]
        public int ApplicationModuleGroupId { get; set; }
    }
}
