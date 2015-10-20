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

        [MaxLength(150)]
        public string Password { get; set; }

        public int SaintId { get; set; }

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        public virtual ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }

        [NotMapped]
        public bool IsAdministrator
        {
            get
            {
                if (ApplicationUserRoles == null)
                    return false;

                var userRoles = ApplicationUserRoles;

                foreach (var userRole in userRoles)
                {
                    if (userRole.Role.IsAdministrator)
                        return true;
                }

                return false;
            }
        }

        public bool HasPrivilege(string privilegeKey)
        {
            if (ApplicationUserRoles == null) 
                return false;

            var userRoles = ApplicationUserRoles;

            foreach (var userRole in userRoles)
            {
                if (userRole.Role.IsAdministrator)
                    return true;
                else
                {
                    if (userRole.Role.HasPrivilege(privilegeKey))
                        return true;
                }
            }

            return false;
        }

    }

}
