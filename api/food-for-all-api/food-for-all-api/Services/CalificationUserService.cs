using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class CalificationUserService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private UserService userService = new UserService();

        public CalificationUserService()
        {

        }

        public CalificationUser create(CalificationUser calificationUser)
        {
            foodForAllContext.CalificationUser.Add(calificationUser);
            foodForAllContext.SaveChanges();

            int star = starByIdUser(calificationUser.IdUser);

            userService.updateStarById(calificationUser.IdUser, star);

            return calificationUser;
        }

        public CalificationUser findById(int id)
        {
            CalificationUser calificationUser = (from cu in foodForAllContext.CalificationUser where cu.Id == id select cu).FirstOrDefault();

            return calificationUser;
        }

        public CalificationUser destroyById(int id)
        {
            CalificationUser calificationUser = (from cu in foodForAllContext.CalificationUser where cu.Id == id select cu).FirstOrDefault();

            foodForAllContext.CalificationUser.Remove(calificationUser);
            foodForAllContext.SaveChanges();

            int star = starByIdUser(calificationUser.IdUser);

            userService.updateStarById(calificationUser.IdUser, star);

            calificationUser = findById(id);

            return calificationUser;
        }

        public int starByIdUser(int idUser)
        {
            int star = 0;

            List<Group> groups = (from cu in foodForAllContext.CalificationUser
                                  where cu.IdUser == idUser
                                  group cu by cu.Calification into c
                                  select new Group()
                                  {
                                      Key = c.Key,
                                      Count = c.Count()
                                  }).ToList();

            Group group = groups.OrderByDescending(g => g.Count).FirstOrDefault();

            star = group != null ? Convert.ToInt32(group.Key) : 0;

            return star;
        }

        public CalificationUser findByIdUserAndIdUserCalification(int idUser, int idUserCalification)
        {
            CalificationUser calificationUser = (from cu in foodForAllContext.CalificationUser where cu.IdUser == idUser && cu.IdUserCalification == idUserCalification select cu).FirstOrDefault();

            return calificationUser;
        }
    }
}
