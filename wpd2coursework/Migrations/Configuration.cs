namespace IP3Latest.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using wpd2coursework.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<wpd2coursework.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(wpd2coursework.Models.ApplicationDbContext context)
        {
            // Seed initial data only if the database is empty
            if (!context.Users.Any())
            {
                var adminEmail = "admin@admin.com";
                var adminUserName = adminEmail;
                var adminPassword = "Admin_1";
                string adminRole = "Store Foremen";

                var salesEmail = "salesass@salesass.com";
                var salesUserName = salesEmail;
                var salesPassword = "Sales_1";
                string salesRole = "Sales Assistant";

                var businessEmail = "business@buisiness.com";
                var businessUserName = businessEmail;
                var businessPassword = "Business_1";
                string businessRole = "Business Customer";

                CreateAdminUser(context, adminEmail, adminUserName, adminPassword, adminRole);
                CreateSalesUser(context, salesEmail, salesUserName, salesPassword, salesRole);
                CreateBusinessUser(context, businessEmail, businessUserName, businessPassword, businessRole);
            }
        }

        private void CreateAdminUser(wpd2coursework.Models.ApplicationDbContext context, string adminEmail, string adminUserName, string adminPassword, string adminRole)
        {
            // Create the "admin" user
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,

                Email = adminEmail,
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            // Create the "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // Add the "admin" user to "Administrator" role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void CreateSalesUser(wpd2coursework.Models.ApplicationDbContext context, string salesEmail, string salesUserName, string salesPassword, string salesRole)
        {
            // Create the "admin" user
            var salesUser = new ApplicationUser
            {
                UserName = salesUserName,

                Email = salesEmail,
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var userCreateResult = userManager.Create(salesUser, salesPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            // Create the "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(salesRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // Add the "admin" user to "Administrator" role
            var addDocAuthRoleResult = userManager.AddToRole(salesUser.Id, salesRole);
            if (!addDocAuthRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addDocAuthRoleResult.Errors));
            }
        }

        private void CreateBusinessUser(wpd2coursework.Models.ApplicationDbContext context, string businessEmail, string businessUserName, string businessPassword, string businessRole)
        {
            // Create the "admin" user
            var businessUser = new ApplicationUser
            {
                UserName = businessUserName,

                Email = businessEmail,
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var userCreateResult = userManager.Create(businessUser, businessPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            // Create the "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(businessRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // Add the "admin" user to "Administrator" role
            var addDistributeeRoleResult = userManager.AddToRole(businessUser.Id, businessRole);
            if (!addDistributeeRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addDistributeeRoleResult.Errors));
            }
        }
    }
}
