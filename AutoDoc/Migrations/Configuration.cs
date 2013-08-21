namespace AutoDoc.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<AutoDoc.Models.AutoDocContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AutoDoc.Models.AutoDocContext context)
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfiles", "UserId", "UserLogin", autoCreateTables: true);
            }

            SeedMembership();
        }

        private void SeedMembership()
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists("�������������")) roles.CreateRole("�������������");
            if (!roles.RoleExists("������������")) roles.CreateRole("������������");
            if (!roles.RoleExists("�������������")) roles.CreateRole("�������������");
            if (!roles.RoleExists("������� �����")) roles.CreateRole("������� �����");

            if (membership.GetUser("Admin", false) == null)
            {
                var UserParams = new Dictionary<string, object>();
                UserParams.Add("FirstName", "�����");
                UserParams.Add("MiddleName", "�����");
                UserParams.Add("LastName", "�����");

                membership.CreateUserAndAccount("admin", "admin_z305", UserParams);          
            }
            
            if (!roles.GetRolesForUser("admin").Contains("�������������")) roles.AddUsersToRoles(new[] { "admin" }, new[] { "�������������" });
        }
    }
}
