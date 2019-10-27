using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class StockImageService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public StockImageService()
        {

        }

        public StockImage create(StockImage stockImage)
        {
            foodForAllContext.StockImage.Add(stockImage);
            foodForAllContext.SaveChanges();

            return stockImage;
        }

        public StockImage findById(int id)
        {
            StockImage stockImage = (from s in foodForAllContext.StockImage where s.Id == id select s).FirstOrDefault();

            return stockImage;
        }

        public StockImage destroyById(int id)
        {
            StockImage stockImage = (from s in foodForAllContext.StockImage where s.Id == id select s).FirstOrDefault();

            foodForAllContext.StockImage.Remove(stockImage);
            foodForAllContext.SaveChanges();

            stockImage = findById(id);

            return stockImage;
        }

        public List<StockImage> findByIdStock(int idStock)
        {
            List<StockImage> stockImages = (from s in foodForAllContext.StockImage where s.IdStock == idStock select s).ToList();

            return stockImages;
        }
    }
}
