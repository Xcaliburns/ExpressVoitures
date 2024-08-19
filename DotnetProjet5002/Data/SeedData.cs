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
                     CodeVin = "toto",
                     Year = new DateTime(2020, 1, 1),
                     PurchaseDate= new DateTime(2020, 1, 1),
                     PurchasePrice= 10000,                    
                     Brand = "renault",
                     Model = "twingo",
                     Finish = "electrique",
                     Description = "voiture neuve",
                     ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.largus.fr%2Factualite-automobile%2Frenault-twingo-electrique-2020-les-prix-de-la-nouvelle-twingo-ze-10300000.html&psig=AOvVaw0",
                     Availability = false,
                     AvailabilityDate = new DateTime(2020, 1, 1),
                     Selled = false

                 },
                  new Vehicle
                  {
                      CodeVin = "titi",
                      Year = new DateTime(2022, 1, 1),
                      Brand = "renault",
                      Model = "twingo",
                      Finish = "electrique",
                      Description = "voiture neuve",
                      ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.largus.fr%2Factualite-automobile%2Frenault-twingo-electrique-2020-les-prix-de-la-nouvelle-twingo-ze-10300000.html&psig=AOvVaw0",
                      Availability = false,
                      AvailabilityDate = new DateTime(2020, 1, 1),
                      Selled = false

                  }
                    );
                context.SaveChanges();
            }
        }

    }
}
