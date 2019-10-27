using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class ListBlackService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public ListBlackService()
        {

        }

        public ListBlack create(ListBlack listBlack)
        {
            foodForAllContext.ListBlack.Add(listBlack);
            foodForAllContext.SaveChanges();

            return listBlack;
        }

        public ListBlack findByUser(User user)
        {
            ListBlack listBlack = (from lb in foodForAllContext.ListBlack
                                   where lb.OneSignalPlayerId == user.OneSignalPlayerId || lb.Phone == user.Phone || lb.Email == user.Email
                                   select lb).FirstOrDefault();

            return listBlack;
        }
    }
}
