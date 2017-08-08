using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCore.API.Entites;

namespace NetCore.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfinterest.Add(pointOfInterest);
        }

        public bool CityExists(int cityId) 
        {
            return _context.Cities.Any(x => x.Id == cityId);
        }

        public IEnumerable<City> GetCities() 
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest = false)
        {
            if (includePointsOfInterest)
                return _context.Cities.Include(c => c.PointsOfinterest).Where(x => x.Id == cityId).FirstOrDefault();

            return _context.Cities.Where(x => x.Id == cityId).FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterest)
        {
            return _context.PointsOfInterest.Where(x => x.CityId == cityId && x.Id == pointOfInterest).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest.Where(x => x.CityId == cityId).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
