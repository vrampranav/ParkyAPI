using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;
        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }
        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }
        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(park => park.Id == nationalParkId);
        }
        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(park => park.Name).ToList();
        }
        public bool NationalParkExists(int nationalParkId)
        {
            return _db.NationalParks.Any(park => park.Id == nationalParkId);
        }
        public bool NationalParkExists(string nationalParkName)
        {
            return _db.NationalParks.Any(park => park.Name.ToLower().Trim().Equals(nationalParkName.ToLower().Trim()));
        }
        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}
