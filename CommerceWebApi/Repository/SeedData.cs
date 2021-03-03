using CommerceWebApi.Contraints;
using CommerceWebApi.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace CommerceWebApi.Repository
{
    public class SeedData
    {
        public SeedData(UserManager<ApplicationUser> userManager)
        {
        }

        public static void DataControl(IApplicationBuilder app)
        {
            var scopeeee = app.ApplicationServices.CreateScope();
            ApplicationDbContext context = scopeeee.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            UserManager<ApplicationUser> userManager = scopeeee.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            List<IdentityRole> roleList;
            if (!context.Roles.Any())
            {
                roleList = new List<IdentityRole>()
                {
                    new IdentityRole {  Name ="CategoryManagement"},
                    new IdentityRole {  Name ="product_view" },
                    new IdentityRole {  Name ="Normal" }
                };
                context.Roles.AddRange(roleList);
                context.SaveChanges();
            }
            else
            {
                roleList = context.Roles.ToList();
            }

            List<ApplicationUser> userList;
            if (!context.Users.Any())
            {
                userList = new List<ApplicationUser>()
                {
                    new ApplicationUser {  UserName ="user1", Email="user1@user.com" },
                    new ApplicationUser {  UserName ="user2", Email="user2@user.com"}
                };
                IdentityResult resultUserOne = userManager.CreateAsync(userList[0], "Password@123").Result;
                IdentityResult resultUserTwo = userManager.CreateAsync(userList[1], "Password@123").Result;
            }
            else
            {
                userList = context.Users.ToList();
            }

            if (!context.UserRoles.Any())
            {
                var user1Role1 = new IdentityUserRole<string>
                {
                    UserId = context.Users.Single(r => r.Email == userList[0].Email).Id,
                    RoleId = context.Roles.Single(r => r.Name == UserRoles.Normal).Id
                };
                context.UserRoles.Add(user1Role1);

                var user1Role2 = new IdentityUserRole<string>
                {
                    UserId = context.Users.Single(r => r.Email == userList[0].Email).Id,
                    RoleId = context.Roles.Single(r => r.Name == UserRoles.CategoryManagement).Id
                };
                context.UserRoles.Add(user1Role2);

                var user2Role1 = new IdentityUserRole<string>
                {
                    UserId = context.Users.Single(r => r.Email == userList[1].Email).Id,
                    RoleId = context.Roles.Single(r => r.Name == UserRoles.ProductView).Id
                };
                var user2Role2 = new IdentityUserRole<string>
                {
                    UserId = context.Users.Single(r => r.Email == userList[1].Email).Id,
                    RoleId = context.Roles.Single(r => r.Name == UserRoles.CategoryManagement).Id
                };
                context.UserRoles.Add(user2Role1);
                context.UserRoles.Add(user2Role2);
                context.SaveChanges();
            }

            var categoryList = new List<Category>();
            if (!context.Categories.Any())
            {
                var mainCategoryList = new List<Category>()
                {
                    new Category{  Name ="Elektronik", ParentId = null },
                    new Category{  Name ="Oto", ParentId = null },
                    new Category{  Name ="SüperMarket", ParentId = null },
                };
                context.Categories.AddRange(mainCategoryList);
                context.SaveChanges();

                categoryList = new List<Category>()
                {
                    new Category{  Name ="Bilgisayar", ParentId = mainCategoryList[0].Id },
                    new Category{  Name ="Telefon", ParentId = mainCategoryList[0].Id },
                    new Category{  Name ="Televizyon", ParentId = mainCategoryList[0].Id },
                    new Category{  Name ="Lastik", ParentId =mainCategoryList[1].Id },
                    new Category{  Name ="Motor Yağı", ParentId = mainCategoryList[1].Id },
                    new Category{  Name ="Akü", ParentId = mainCategoryList[1].Id },
                    new Category{  Name ="Deterjan", ParentId = mainCategoryList[2].Id },
                    new Category{  Name ="Bebek Bezi", ParentId = mainCategoryList[2].Id },
                    new Category{  Name ="Kağıt Havlu", ParentId = mainCategoryList[2].Id}
                };
                context.Categories.AddRange(categoryList);
                context.SaveChanges();
            }
            else
            {
                categoryList = context.Categories.Where(p => p.ParentId > 0).ToList();
            }

            if (!context.Products.Any())
            {
                var productList = new List<Product>() {
                    new Product{ Category = categoryList[0] , Name="HP 250 G7 Intel Core i3 ", IsActive = true, Price =5000,  Description="HP 250 G7 Dizüstü Bilgisayar", ImageUrl="image1.png" },
                    new Product{ Category =categoryList[1], Name="Black Shark 3 128 GB ", IsActive = true, Price =7000,  Description="Black Shark 3 128 GB", ImageUrl="image2.png" },
                    new Product{ Category =categoryList[2], Name="Samsung 50TU7000 50 127 Ekran Uydu Alıcılı ", IsActive = true, Price = 4750,  Description="Samsung 50TU7000 50 127 Ekran Uydu Alıcılı", ImageUrl="image3.png" },

                    new Product{ Category = categoryList[3], Name="Petlas 195/65 R15", IsActive = true, Price =250,  Description="Petlas 195/65 R15", ImageUrl="image4.png" },
                    new Product{ Category = categoryList[4], Name="Shell Helix HX6 10W40 Yarı Sentetik Motor Yağı ", IsActive = true, Price =80,  Description="Shell Helix HX6 10W40 Yarı Sentetik Motor Yağı", ImageUrl="image5.png" },
                    new Product{ Category = categoryList[5], Name="AutoCsi 1200 Amper Akü Takviye Kablosu", IsActive = true, Price =45,  Description="AutoCsi 1200 Amper Akü Takviye Kablosu" , ImageUrl="image6.png"},

                    new Product{ Category = categoryList[6], Name="Omo Sık Yıkananlar Sıvı Çamaşır Deterjanı 2470 ML", IsActive = true, Price =30,  Description="Omo Sık Yıkananlar Sıvı Çamaşır Deterjanı 2470 ML" , ImageUrl="image7.png"},
                    new Product{ Category = categoryList[7], Name="Sleepy Natural Bebek Bezi 6 Numara ", IsActive = true, Price =120,  Description="Sleepy Natural Bebek Bezi 6 Numara ", ImageUrl="image8.png" },
                    new Product{ Category = categoryList[8], Name="Selpak Kağıt Havlu 12'li", IsActive = true, Price =32,  Description="Selpak Kağıt Havlu 12li" , ImageUrl="image9.png"},
                };
                context.Products.AddRange(productList);
                context.SaveChanges();
            }
        }
    }
}