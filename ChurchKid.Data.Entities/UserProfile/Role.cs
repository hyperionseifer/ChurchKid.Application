using ChurchKid.Common.Utilities.Cryptography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChurchKid.Data.Entities.UserProfile
{
    public class Role : NamedEntity
    {

        [Key]
        public int RoleId { get; set; }

        public virtual ICollection<RolePrivilege> RolePrivileges { get; set; }

        public ICollection<string> Privileges()
        {
            var privileges = new List<string>();

            var rolePrivileges = (from privilege in RolePrivileges
                                  select privilege).ToList();

            foreach (var rolePrivilege in rolePrivileges)
            {
                var privilegeValue = Cryptographer.Decrypt(rolePrivilege.Privilege.PrivilegeOption);
                if (!privileges.Contains(privilegeValue))
                    privileges.Add(privilegeValue);
            }

            return privileges;
        }

        public bool HasPrivilege(string privilegeKey)
        {
            var decryptedPrivilege = Cryptographer.Decrypt(privilegeKey);
            return Privileges().Contains(decryptedPrivilege);
        }

        [NotMapped]
        public bool IsAdministrator
        {
            get
            {
                return Privileges().Contains("UNRESTRICTED");
            }
        }

    }
}
