using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class LocationService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public LocationService()
        {

        }

        public Location create(Location location)
        {
            location.Status = true;

            foodForAllContext.Location.Add(location);
            foodForAllContext.SaveChanges();

            return location;
        }

        public Location findById(int id)
        {
            Location location = (from l in foodForAllContext.Location where l.Id == id select l).FirstOrDefault();

            return location;
        }

        public Location findByIdUserAndMain(int idUser)
        {
            Location location = (from l in foodForAllContext.Location where l.IdUser == idUser && l.IsMain == true && l.Status == true select l).FirstOrDefault();

            return location;
        }

        public Location findByIdUserAndAddress(int idUser, string address)
        {
            Location location = (from l in foodForAllContext.Location where l.IdUser == idUser && l.Address.ToLower() == address.ToLower() && l.Status == true select l).FirstOrDefault();

            return location;
        }

        public List<Location> findByIdUser(int idUser)
        {
            List<Location> locations = (from l in foodForAllContext.Location where l.IdUser == idUser && l.Status == true orderby l.Id select l).ToList();

            return locations;
        }

        public List<Location> findAllStockAvailable(string search, bool? isSearchLocation)
        {
            List<Location> locations = new List<Location>();

            if (string.IsNullOrEmpty(search))
            {
                List<int> userIds = (from sa in foodForAllContext.StockAvailable
                                     join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                                     join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                     select s.IdUser).ToList();

                locations = (from l in foodForAllContext.Location
                             join u in foodForAllContext.User on l.IdUser equals u.Id
                             where l.IsMain == true && userIds.Contains(l.IdUser) && u.Status == true && l.Status == true
                             select new Location()
                             {
                                 Address = l.Address,
                                 Country = l.Country,
                                 Id = l.Id,
                                 IdUser = l.IdUser,
                                 IsMain = l.IsMain,
                                 Latitude = l.Latitude,
                                 Longitude = l.Longitude,
                                 Status = l.Status,
                                 IdUserNavigation = u
                             }).ToList();
            }
            else
            {
                if (isSearchLocation == true)
                {
                    List<int> userIds = (from sa in foodForAllContext.StockAvailable
                                         join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                                         join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                         select s.IdUser).ToList();

                    locations = (from l in foodForAllContext.Location
                                 join u in foodForAllContext.User on l.IdUser equals u.Id
                                 where l.IsMain == true && userIds.Contains(l.IdUser) && l.Address.ToLower().Contains(search.ToLower()) && u.Status == true && l.Status == true
                                 select new Location()
                                 {
                                     Address = l.Address,
                                     Country = l.Country,
                                     Id = l.Id,
                                     IdUser = l.IdUser,
                                     IsMain = l.IsMain,
                                     Latitude = l.Latitude,
                                     Longitude = l.Longitude,
                                     Status = l.Status,
                                     IdUserNavigation = u
                                 }).ToList();
                }
                else
                {
                    List<int> userIds = (from sa in foodForAllContext.StockAvailable
                                         join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                                         join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                         join u in foodForAllContext.User on s.IdUser equals u.Id
                                         where u.UserName.ToLower().Contains(search.ToLower())
                                         select s.IdUser).ToList();

                    locations = (from l in foodForAllContext.Location
                                 join u in foodForAllContext.User on l.IdUser equals u.Id
                                 where l.IsMain == true && userIds.Contains(l.IdUser) && u.Status == true && l.Status == true
                                 select new Location()
                                 {
                                     Address = l.Address,
                                     Country = l.Country,
                                     Id = l.Id,
                                     IdUser = l.IdUser,
                                     IsMain = l.IsMain,
                                     Latitude = l.Latitude,
                                     Longitude = l.Longitude,
                                     Status = l.Status,
                                     IdUserNavigation = u
                                 }).ToList();
                }

            }

            return locations;
        }

        public Location updateIsMainById(Location location)
        {
            Location locationExisting = (from l in foodForAllContext.Location where l.Id == location.Id select l).FirstOrDefault();

            locationExisting.IsMain = location.IsMain;

            foodForAllContext.SaveChanges();

            return location;
        }

        public Location deleteById(int id)
        {
            Location location = (from l in foodForAllContext.Location where l.Id == id select l).FirstOrDefault();

            location.Status = false;

            foodForAllContext.SaveChanges();

            return location;
        }
    }
}
