using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class EventLogService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public EventLogService()
        {

        }

        public EventLog create(EventLog eventLog)
        {
            eventLog.Date = DateTime.Now;

            foodForAllContext.EventLog.Add(eventLog);
            foodForAllContext.SaveChanges();

            return eventLog;
        }
    }
}
