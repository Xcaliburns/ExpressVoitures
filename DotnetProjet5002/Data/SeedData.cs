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
                     Brand = "renault",
                     Model = "twingo",
                     Finish = "electrique",
                     Availability = false,

                 },
                  new Vehicle
                  {
                      CodeVin = "titi",
                      Year = new DateTime(2022, 1, 1),
                      Brand = "renault",
                      Model = "twingo",
                      Finish = "electrique",
                      Availability = false,

                  }
                    );
                context.SaveChanges();
            }
        }

    }
}
