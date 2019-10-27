using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class StockAvailableService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public StockAvailableService()
        {

        }

        public StockAvailable create(StockAvailable stockAvailable)
        {
            stockAvailable.DateOfAdmission = DateTime.Now;
            foodForAllContext.StockAvailable.Add(stockAvailable);
            foodForAllContext.SaveChanges();

            return stockAvailable;
        }

        public StockAvailable findByIdStock(int idStock)
        {
            StockAvailable stockAvailable = (from sa in foodForAllContext.StockAvailable where sa.IdStock == idStock select sa).FirstOrDefault();

            return stockAvailable;
        }

        public StockAvailable destroyByIdStock(int idStock)
        {
            StockAvailable stockAvailable = (from sa in foodForAllContext.StockAvailable where sa.IdStock == idStock select sa).FirstOrDefault();

            foodForAllContext.StockAvailable.Remove(stockAvailable);
            foodForAllContext.SaveChanges();

            stockAvailable = (from sa in foodForAllContext.StockAvailable where sa.IdStock == idStock select sa).FirstOrDefault();

            return stockAvailable;
        }
    }
}
