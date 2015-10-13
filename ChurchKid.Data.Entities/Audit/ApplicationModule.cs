using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChurchKid.Data.Entities.Audit
{
    public class ApplicationModule : NamedEntity
    {

        [Key]
        public int ApplicationModuleId { get; set; }

        [ForeignKey("ApplicationModuleGroup")]
        public int ApplicationModuleGroupId { get; set; }

        public virtual ApplicationModuleGroup ApplicationModuleGroup { get; set; }
    
    }
}
