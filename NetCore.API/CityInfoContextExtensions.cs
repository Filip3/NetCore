using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.API.Entites;

namespace NetCore.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if(context.Cities.Any())
            {
                return;
            }

            // initial seed data for cities
            var cities = new List<City>()
            {
                new City()
                {
                    Name = "Amsterdam",
                    Description = "Bacon ipsum dolor amet prosciutto drumstick ham.",
                    PointsOfinterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Bacon Prime",
                            Description = "Shankle chuck kielbasa fatback ham hock cow venison shank strip steak ground round."
                        },
                        new PointOfInterest()
                        {
                            Name = "Bacon",
                            Description = "Drumstick strip steak cupim, pork chop sirloin meatloaf tri-tip jerky."
                        }
                    }
                },
                new City()
                {
                    Name = "London",
                    Description = "Ham hock meatloaf pastrami capicola bacon pork loin.",
                    PointsOfinterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "London Bacon Point One",
                            Description = "Burgdoggen beef shoulder kevin beef ribs swine"
                        },
                        new PointOfInterest()
                        {
                            Name = "London Bacon Point Two",
                            Description = "Landjaeger sirloin drumstick cupim chuck, corned beef pork chop."
                        }
                    }
                }
            };

            context.AddRange(cities);
            context.SaveChanges();
        }
    }
}
