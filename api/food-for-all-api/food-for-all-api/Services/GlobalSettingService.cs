using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class GlobalSettingService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public GlobalSettingService()
        {

        }

        public GlobalSetting findById(int id)
        {
            GlobalSetting globalSetting = (from gs in foodForAllContext.GlobalSetting where gs.Id == id select gs).FirstOrDefault();

            return globalSetting;
        }
    }
}
