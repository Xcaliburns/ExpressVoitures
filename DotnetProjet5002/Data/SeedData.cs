using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotnetProjet5.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace DotnetProjet5.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any vehicle.
                if (context.Vehicle.Any())
                {
                    return;   
                }
                context.Vehicle.AddRange(
                    new Vehicle
                    {
                        CodeVin = "toto5555555555555",
                        Year = new DateTime(2020, 1, 1),
                        PurchaseDate = new DateTime(2020, 1, 1),
                        PurchasePrice = 10000,
                        Brand = "renault",
                        Model = "twingo",
                        Finish = "electrique",
                        Description = "voiture neuve",
                        ImageUrl = "https://www.example.com/twingo.jpg",
                        Availability = true,
                        AvailabilityDate = new DateTime(2020, 1, 1),
                        Selled = false
                    },
                    new Vehicle
                    {
                        CodeVin = "toto6666666666666",
                        Year = new DateTime(2019, 1, 1),
                        PurchaseDate = new DateTime(2019, 1, 1),
                        PurchasePrice = 15000,
                        Brand = "peugeot",
                        Model = "208",
                        Finish = "diesel",
                        Description = "voiture d'occasion",
                        ImageUrl = "https://www.example.com/208.jpg",
                        Availability = true,
                        AvailabilityDate = new DateTime(2019, 1, 1),
                        Selled = false
                    },
                    new Vehicle
                    {
                        CodeVin = "toto7777777777777",
                        Year = new DateTime(2018, 1, 1),
                        PurchaseDate = new DateTime(2018, 1, 1),
                        PurchasePrice = 20000,
                        Brand = "citroen",
                        Model = "c3",
                        Finish = "hybride",
                        Description = "voiture hybride",
                        ImageUrl = "https://www.example.com/c3.jpg",
                        Availability = true,
                        AvailabilityDate = new DateTime(2018, 1, 1),
                        Selled = false
                    },
                    new Vehicle
                    {
                        CodeVin = "toto8888888888888",
                        Year = new DateTime(2021, 1, 1),
                        PurchaseDate = new DateTime(2021, 1, 1),
                        PurchasePrice = 25000,
                        Brand = "tesla",
                        Model = "model 3",
                        Finish = "electrique",
                        Description = "voiture électrique",
                        ImageUrl = "https://www.example.com/model3.jpg",
                        Availability = true,
                        AvailabilityDate = new DateTime(2021, 1, 1),
                        Selled = false
                    }
                );
                context.SaveChanges();
            }

            
            await CreateDefaultAdmin(serviceProvider);
        }

        private static async Task CreateDefaultAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminRole = "Admin";
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123456";

           
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}
