using ChurchKid.Common.Audit;
using ChurchKid.Common.Resources;
using ChurchKid.Common.Utilities.Cryptography;
using ChurchKid.Data.Entities.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace ChurchKid.Data.Seed
{
    public class Users : ISeeder
    {

        private DatabaseConnection connection;

        public Users(DatabaseConnection connection)
        {
            this.connection = connection;
            SeedData = Properties.Resources.Users;
        }

        public string SeedData { get; set; }

        public void Seed()
        {
            if (connection == null)
                return;

            try
            {
                if (!connection.ApplicationUsers.Any())
                {
                    var data = XElement.Parse(SeedData);
                    var systemUsers = from user in data.Elements("user")
                                      select new
                                      {
                                          Username = (string)user.Element("username"),
                                          Password = (string)user.Element("password"),
                                          FirstName = (string)user.Element("firstName"),
                                          MiddleName = (string)user.Element("middleName"),
                                          LastName = (string)user.Element("lastName"),
                                          NickName = (string)user.Element("nickName"),
                                          Email = (string)user.Element("email"),
                                          Roles = (from role in user.Element("roles").Elements("role")
                                                   select new
                                                   {
                                                       Role = (string)role
                                                   }).ToList()
                                      };


                    if (systemUsers.Any())
                    {
                        var persistedRoles = connection.Roles.ToList();
                        var administratorId = 0;

                        using (var transaction = new TransactionScope())
                        {
                            foreach (var systemUser in systemUsers)
                            {
                                var persistedUser = connection.ApplicationUsers.Add(new ApplicationUser()
                                {
                                   Username = systemUser.Username,
                                   Password = Cryptographer.Encrypt(systemUser.Password),
                                   FirstName = systemUser.FirstName,
                                   MiddleName = systemUser.MiddleName,
                                   LastName = systemUser.LastName,
                                   NickName = systemUser.NickName,
                                   Email = systemUser.Email
                                });

                                connection.SaveChanges();

                                if (administratorId <= 0)
                                {
                                    if ("churchkid".Equals(persistedUser.Username, StringComparison.InvariantCultureIgnoreCase))
                                        administratorId = persistedUser.UserId;
                                }

                                if (administratorId > 0)
                                {
                                    persistedUser.CreatedById = administratorId;
                                    connection.SaveChanges();
                                }

                                var systemUserRoles = new List<ApplicationUserRole>();

                                foreach (var role in systemUser.Roles)
                                {
                                    var encryptedRole = Cryptographer.Encrypt(role.Role);
                                    var persistedRole = persistedRoles.FirstOrDefault(p => encryptedRole.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase));
                                    if (persistedRole != null)
                                        systemUserRoles.Add(new ApplicationUserRole()
                                        {
                                            RoleId = persistedRole.RoleId,
                                            UserId = persistedUser.UserId
                                        });
                                }

                                if (systemUserRoles.Any())
                                {
                                    connection.ApplicationUserRoles.AddRange(systemUserRoles);
                                    connection.SaveChanges();
                                }
                                else
                                    throw new Exception(string.Format(ApplicationStrings.errNoUserRoleSpecified, systemUser.Username));
                            }

                            transaction.Complete();
                            Logger.Write(string.Format(ApplicationStrings.msgSeededData, ApplicationStrings.dataUsers));
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
