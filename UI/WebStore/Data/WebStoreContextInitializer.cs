using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;

namespace WebStore.Data
{
    public class WebStoreContextInitializer
    {
        private readonly WebStoreContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public WebStoreContextInitializer(WebStoreContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            await db.Database.MigrateAsync();
            await InitializeIdentityAsync();
            if (await db.Products.AnyAsync()) return;

            using(var transaction = db.Database.BeginTransaction())
            {
                await db.Sections.AddRangeAsync(TestData.Sections);
                await db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await db.SaveChangesAsync();
                await db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                transaction.Commit();
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                await db.Brands.AddRangeAsync(TestData.Brands);
                await db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await db.SaveChangesAsync();
                await db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                transaction.Commit();
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                await db.Products.AddRangeAsync(TestData.Products);
                await db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await db.SaveChangesAsync();
                await db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");
                transaction.Commit();
            }
        }
        public async Task InitializeIdentityAsync()
        {
            if (!await roleManager.RoleExistsAsync(User.RoleUser))
                await roleManager.CreateAsync(new IdentityRole(User.RoleUser));

            if (!await roleManager.RoleExistsAsync(User.RoleAdmin))
                await roleManager.CreateAsync(new IdentityRole(User.RoleAdmin));

            if(await userManager.FindByNameAsync(User.AdminUserName) == null)
            {
                var admin = new User
                {
                    UserName = User.AdminUserName,
                    Email = $"{User.AdminUserName}@server.ru"
                };
                var creation_result = await userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded) await userManager.AddToRoleAsync(admin, User.RoleAdmin);
            }
        }
    }
}
