using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class CalificationStockService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private StockService stockService = new StockService();

        public CalificationStockService()
        {

        }

        public CalificationStock create(CalificationStock calificationStock)
        {
            foodForAllContext.CalificationStock.Add(calificationStock);
            foodForAllContext.SaveChanges();

            int star = starByIdStock(calificationStock.IdStock);

            stockService.updateStarById(calificationStock.IdStock, star);

            return calificationStock;
        }

        public CalificationStock findById(int id)
        {
            CalificationStock calificationStock = (from cs in foodForAllContext.CalificationStock where cs.Id == id select cs).FirstOrDefault();

            return calificationStock;
        }

        public CalificationStock destroyById(int id)
        {
            CalificationStock calificationStock = (from cs in foodForAllContext.CalificationStock where cs.Id == id select cs).FirstOrDefault();

            foodForAllContext.CalificationStock.Remove(calificationStock);
            foodForAllContext.SaveChanges();

            int star = starByIdStock(calificationStock.IdStock);

            stockService.updateStarById(calificationStock.IdStock, star);

            calificationStock = findById(id);

            return calificationStock;
        }

        public int starByIdStock(int idStock)
        {
            int star = 0;

            List<Group> groups = (from cs in foodForAllContext.CalificationStock
                                  where cs.IdStock == idStock
                                  group cs by cs.Calification into c
                                  select new Group()
                                  {
                                      Key = c.Key,
                                      Count = c.Count()
                                  }).ToList();

            Group group = groups.OrderByDescending(g => g.Count).FirstOrDefault();

            star = group != null ? Convert.ToInt32(group.Key) : 0;

            return star;
        }

        public CalificationStock findByIdStockAndIdUserCalification(int idStock, int idUserCalification)
        {
            CalificationStock calificationStock = (from cs in foodForAllContext.CalificationStock where cs.IdStock == idStock && cs.IdUserCalification == idUserCalification select cs).FirstOrDefault();

            return calificationStock;
        }
    }
}
