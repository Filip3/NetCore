using NetCore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDTO> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDTO>()
            {
                new CityDTO()
                {
                    Id = 1,
                    Name = "Amsterdam",
                    Description = "das da d asd asasdda",
                    PointsOfInterest = new List<PointOfInterestDTO>()
                    {
                        new PointOfInterestDTO()
                        {
                            Id = 1,
                            Name = "dads",
                            Description = "afadfafda adfad af"
                        },
                        new PointOfInterestDTO()
                        {
                            Id =2,
                            Name = "tryt",
                            Description = "ujhsrger adfad af"
                        }
                    }
                },
                new CityDTO()
                {
                    Id = 2,
                    Name = "London",
                    Description = "hgfs gs s gs gs",
                    PointsOfInterest = new List<PointOfInterestDTO>()
                    {
                        new PointOfInterestDTO()
                        {
                            Id = 1,
                            Name = "rgrrfrf",
                            Description = "kiikkiiku adfad af"
                        },
                        new PointOfInterestDTO()
                        {
                            Id =2,
                            Name = "qwdqwq",
                            Description = "cxcvzvv adfad af"
                        }
                    }
                }
            };
        }
    }
}
