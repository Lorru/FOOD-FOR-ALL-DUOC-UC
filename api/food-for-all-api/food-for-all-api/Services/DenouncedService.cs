using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class DenouncedService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private UserService userService = new UserService();
        private ListBlackService listBlackService = new ListBlackService();

        public DenouncedService()
        {

        }

        public Denounced create(Denounced denounced)
        {
            foodForAllContext.Denounced.Add(denounced);
            foodForAllContext.SaveChanges();

            int complaints = findByIdUser(denounced.IdUser);

            if (complaints == 5)
            {
                User user = userService.findById(denounced.IdUser);

                ListBlack listBlack = new ListBlack();

                listBlack.IdUser = user.Id;
                listBlack.OneSignalPlayerId = user.OneSignalPlayerId;
                listBlack.Phone = user.Phone;
                listBlack.Email = user.Email;

                listBlackService.create(listBlack);

                userService.deleteById(user.Id);
            }

            return denounced;
        }

        public int findByIdUser(int idUser)
        {
            int complaints = (from d in foodForAllContext.Denounced where d.IdUser == idUser select d).ToList().Count;

            return complaints;
        }

        public Denounced findByIdUserAndIdUserAccuser(int idUser, int idUserAccuser)
        {
            Denounced denounced = (from d in foodForAllContext.Denounced where d.IdUser == idUser && d.IdUserAccuser == idUserAccuser select d).FirstOrDefault();

            return denounced;
        }

    }
}
