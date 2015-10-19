using ChurchKid.Common.Audit;
using ChurchKid.Common.Utilities.Cryptography;
using ChurchKid.Data.Entities.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class RolesAndPrivileges : ISeeder
    {

        private DatabaseConnection connection;

        public RolesAndPrivileges(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.Roles;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            var data = XElement.Parse(SeedData);
            var systemRoles = from role in data.Elements("role")
                              select new
                              {
                                  Name = (string)role.Element("name"),
                                  AllowDelete = (bool)role.Element("allowDelete"),
                                  AdministrativeRole = (bool)role.Element("administratorRole"),
                                  Privileges = (from privilege in role.Element("privileges").Elements("privilege")
                                                select new
                                                {
                                                    Key = Cryptographer.Encrypt((string)privilege)
                                                }).ToList()
                              };

            try
            {
                var newPrivileges = new List<Privilege>();

                foreach (var systemRole in systemRoles)
                {
                    foreach (var privilege in systemRole.Privileges)
                    {
                        if (!connection.Privileges.Any(p => privilege.Key.Equals(p.PrivilegeOption, StringComparison.InvariantCultureIgnoreCase)) &&
                            !newPrivileges.Any(p => privilege.Key.Equals(p.PrivilegeOption, StringComparison.InvariantCultureIgnoreCase)))
                            newPrivileges.Add(new Privilege()
                            {
                                PrivilegeOption = privilege.Key
                            });
                    }
                }

                if (newPrivileges.Any())
                {
                    using (var transaction = new TransactionScope())
                    {
                        connection.Privileges.AddRange(newPrivileges);
                        connection.SaveChanges();
                        transaction.Complete();
                    }
                }

                if (!connection.Roles.Any())
                {
                    if (systemRoles.Any())
                    {
                        var persistedPrivileges = connection.Privileges.ToList();

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var systemRole in systemRoles)
                            {
                                var newPersistedRole = connection.Roles.Add(new Role()
                                {
                                    Name = Cryptographer.Encrypt(systemRole.Name),
                                    AllowDelete = systemRole.AllowDelete,
                                    AdministrativeRole = systemRole.AdministrativeRole
                                });

                                connection.SaveChanges();

                                foreach (var privilege in systemRole.Privileges)
                                {
                                    var persistedPrivilege = persistedPrivileges.FirstOrDefault(p => privilege.Key.Equals(p.PrivilegeOption, StringComparison.InvariantCultureIgnoreCase));
                                    connection.RolePrivileges.Add(new RolePrivilege()
                                    {
                                        RoleId = newPersistedRole.RoleId,
                                        PrivilegeId = persistedPrivilege.PrivilegeId
                                    });
                                }

                                connection.SaveChanges();
                            }

                            transaction.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
        }

    }
}
