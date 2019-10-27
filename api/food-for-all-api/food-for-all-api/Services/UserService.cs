using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class UserService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private StockService stockService = new StockService();
        private TokenService tokenService = new TokenService();
        private SystemService systemService = new SystemService();

        public UserService()
        {

        }

        public User findByEmail(string email)
        {
            User user = (from u in foodForAllContext.User where u.Email.ToLower() == email.ToLower() select u).FirstOrDefault();

            return user;
        }

        public User findByEmailWithFacebook(string email)
        {
            User user = (from u in foodForAllContext.User where u.Email.ToLower() == email.ToLower() && u.Status == true select u).FirstOrDefault();

            return user;
        }

        public User findByUserName(string userName)
        {
            User user = (from u in foodForAllContext.User where u.UserName.ToLower() == userName.ToLower() select u).FirstOrDefault();

            return user;
        }

        public User findByUserNameAndPassword(User user)
        {
            User userExisting = (from u in foodForAllContext.User where u.UserName.ToLower() == user.UserName.ToLower() && u.Status == true select u).FirstOrDefault();

            if (userExisting != null)
            {
                bool verify = BCrypt.Net.BCrypt.Verify(user.Password, userExisting.Password);

                if (!verify)
                {
                    userExisting = null;
                }
            }

            return userExisting;
        }

        public User findById(int id)
        {
            User user = (from u in foodForAllContext.User
                         join institution in foodForAllContext.Institution on u.IdInstitution equals institution.Id into ui
                         from i in ui.DefaultIfEmpty()
                         where u.Id == id
                         select new User()
                         {
                             DateOfAdmission = u.DateOfAdmission,
                             OnLine = u.OnLine,
                             Email = u.Email,
                             Id = u.Id,
                             IdInstitution = u.IdInstitution,
                             UserName = u.UserName,
                             Status = u.Status,
                             Star = u.Star,
                             Photo = u.Photo,
                             Phone = u.Phone,
                             Password = u.Password,
                             OneSignalPlayerId = u.OneSignalPlayerId,
                             IsWithFacebook = u.IsWithFacebook,
                             IdUserType = u.IdUserType,
                             IdInstitutionNavigation = i
                         }).FirstOrDefault();

            return user;
        }

        public User create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.DateOfAdmission = DateTime.Now;
            user.Status = true;

            foodForAllContext.User.Add(user);
            foodForAllContext.SaveChanges();

            return user;
        }

        public User updateById(User user)
        {
            User userExisting = (from u in foodForAllContext.User where u.Id == user.Id select u).FirstOrDefault();

            userExisting.Email = user.Email;
            userExisting.Phone = user.Phone;
            userExisting.Photo = user.Photo;
            userExisting.OneSignalPlayerId = user.OneSignalPlayerId;

            foodForAllContext.SaveChanges();

            return userExisting;
        }

        public User updateStarById(int id, int star)
        {
            User userExisting = (from u in foodForAllContext.User where u.Id == id select u).FirstOrDefault();

            userExisting.Star = star;

            foodForAllContext.SaveChanges();

            return userExisting;
        }

        public User updateOnLineById(User user)
        {
            User userExisting = (from u in foodForAllContext.User where u.Id == user.Id select u).FirstOrDefault();

            userExisting.OnLine = user.OnLine;

            foodForAllContext.SaveChanges();

            return userExisting;
        }

        public User updatePasswordById(User user)
        {
            User userExisting = (from u in foodForAllContext.User where u.Id == user.Id select u).FirstOrDefault();

            userExisting.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            foodForAllContext.SaveChanges();

            return userExisting;
        }

        public User findByToken(string tokenString)
        {
            User userExisting = null;

            Token token = tokenService.findByToken(tokenString);

            if (token != null)
            {
                userExisting = findById(token.IdUser);
            }

            return userExisting;
        }

        public User deleteById(int id)
        {
            User user = (from u in foodForAllContext.User where u.Id == id select u).FirstOrDefault();

            user.Status = false;

            foodForAllContext.SaveChanges();

            return user;
        }

        public User findByIdUserBeneficiaryAndMaxStockRetirated(int idUserBeneficiary)
        {
            User user = null;

            List<Group> groups = (from sr in foodForAllContext.StockReceived
                                  where sr.IdUserBeneficiary == idUserBeneficiary
                                  group sr by sr.IdStock into s
                                  select new Group()
                                  {
                                      Key = s.Key,
                                      Count = s.Count()
                                  }).ToList();

            Group group = groups.OrderByDescending(g => g.Count).FirstOrDefault();

            if (group != null)
            {
                Stock stock = stockService.findById(Convert.ToInt32(group.Key));

                user = stock != null ? findById(stock.IdUser) : null;
            }

            

            return user;
        }

        public List<User> findAllUserBeneficiary()
        {
            List<User> users = (from u in foodForAllContext.User where u.IdUserType == 3 && u.Status == true select u).ToList();

            return users;
        }

        public List<User> findByIdUserAndFilterDynamic(int idUser, string searcher, string property, string condition, string value)
        {
            List<User> users = new List<User>();

            Type type = typeof(User);
            PropertyInfo propertyInfo = type.GetProperty(property);

            if (string.IsNullOrEmpty(searcher))
            {
                users = (from u in foodForAllContext.User
                         where u.Id != idUser && u.Status == true && systemService.comparator(condition, propertyInfo.GetValue(u), value)
                         select u).ToList();
            }
            else
            {
                users = (from u in foodForAllContext.User
                         where u.Id != idUser && u.Status == true && u.UserName.ToLower().Contains(searcher.ToLower()) && systemService.comparator(condition, propertyInfo.GetValue(u), value)
                         select u).ToList();
            }

            return users;
        }
    }
}
