using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.API.Entites;

namespace NetCore.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City>  GetCities();
        City GetCity(int cityId, bool includePointsOfInterest);
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterest);
    }
}
