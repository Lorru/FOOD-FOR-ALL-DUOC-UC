using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class TokenService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private GlobalSettingService globalSettingService = new GlobalSettingService();
        private Random random = new Random();

        private int IdDurationToken = 1;
        private int IdCharacterToken = 2;
        private int IdLengthToken = 3;
        private int DurationTokenDefault = 50;
        private int LengthDetault = 50;

        private string CharactersDefault = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public TokenService()
        {

        }

        public Token create(Token token)
        {
            GlobalSetting globalSettingDurationToken = globalSettingService.findById(IdDurationToken);
            GlobalSetting globalSettingCharacterToken = globalSettingService.findById(IdCharacterToken);
            GlobalSetting globalSettingLengthToken = globalSettingService.findById(IdLengthToken);

            Token tokenExisting = findByIdUser(token.IdUser);

            if (tokenExisting != null)
            {
                tokenExisting = deleteById(tokenExisting.Id);
            }

            string characters = globalSettingCharacterToken == null ? CharactersDefault : globalSettingCharacterToken.Value;
            int length = globalSettingLengthToken == null ? LengthDetault : Convert.ToInt32(globalSettingLengthToken.Value);
            int durationToken = globalSettingDurationToken == null ? DurationTokenDefault : Convert.ToInt32(globalSettingDurationToken.Value);

            string tokenString = new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
            tokenString = tokenString + "." + new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());
            tokenString = tokenString + "." + new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)]).ToArray());

            token.ExpirationDate = DateTime.Now.AddMinutes(durationToken);
            token.Token1 = tokenString.ToLower();

            foodForAllContext.Token.Add(token);
            foodForAllContext.SaveChanges();

            return token;
        }

        private Token findByIdUser(int idUser)
        {
            Token token = (from t in foodForAllContext.Token where t.IdUser == idUser select t).FirstOrDefault();

            return token;
        }

        private Token findById(int id)
        {
            Token token = (from t in foodForAllContext.Token where t.Id == id select t).FirstOrDefault();

            return token;
        }

        private Token deleteById(int id)
        {
            Token token = (from t in foodForAllContext.Token where t.Id == id select t).FirstOrDefault();

            foodForAllContext.Token.Remove(token);
            foodForAllContext.SaveChanges();

            token = findById(id);

            return token;
        }

        public Token findByToken(string tokenString)
        {
            Token token = (from t in foodForAllContext.Token where t.Token1.ToLower() == tokenString.ToLower() select t).FirstOrDefault();

            return token;
        }

        public Token findByToken(string tokenString, string host)
        {
            Token token = (from t in foodForAllContext.Token where t.Token1.ToLower() == tokenString.ToLower() && t.Host == host select t).FirstOrDefault();

            if (token != null)
            {
                if (token.Host == host)
                {
                    if (token.ExpirationDate <= DateTime.Now)
                    {
                        token = deleteById(token.Id);
                    }
                }
                else
                {
                    token = null;
                }
            }

            return token;
        }
    }
}
