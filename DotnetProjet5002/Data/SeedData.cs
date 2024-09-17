using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotnetProjet5.Models.Entities;
using System;
using System.Linq;

namespace DotnetProjet5.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new ApplicationDbContext(
             serviceProvider.GetRequiredService<
                 DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any vehicle.
                if (context.Vehicle.Any())
                {
                    return;   // DB has been seeded
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
        }

    }
}
