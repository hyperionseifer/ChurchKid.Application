using ChurchKid.Common.Utilities.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChurchKid.Data.Entities.UserProfile
{
    public class ApplicationUser : NamedPerson
    {

        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        public int SaintId { get; set; }

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }

        [NotMapped]
        private ICollection<string> privileges;

        public void ReloadPrivileges()
        {
            privileges = new List<string>();

            var userRoles = ApplicationUserRoles.ToList();

            foreach (var userRole in userRoles)
            {
                var rolePrivileges = (from privilege in userRole.Role.RolePrivileges
                                      select privilege).ToList();

                foreach (var rolePrivilege in rolePrivileges)
                {
                    var privilegeValue = Cryptographer.Decrypt(rolePrivilege.Privilege.PrivilegeOption);
                    if (!privileges.Contains(privilegeValue))
                        privileges.Add(privilegeValue);
                }
            }
        }

        public bool HasPrivilege(string privilegeKey)
        {
            if (ApplicationUserRoles == null) 
                return false;

            var userRoles = ApplicationUserRoles.ToList();

            var administratorRoles = from role in userRoles
                                     where role.Role.AdministrativeRole == true
                                     select role;

            if (administratorRoles.Any())
                return true;

            if (privileges == null)
                ReloadPrivileges();

            return privileges.Contains(privilegeKey);
        }

    }

}
