using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class StockService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private SystemService systemService = new SystemService();

        public StockService()
        {

        }

        public List<Stock> findByIdUser(int idUser, string searcher)
        {
            List<Stock> stocks = new List<Stock>();

            if (string.IsNullOrEmpty(searcher))
            {
                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where s.IdUser == idUser
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }
            else
            {
                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where s.IdUser == idUser && p.Name.ToLower().Contains(searcher.ToLower())
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }

            return stocks;
        }

        public List<Stock> findByIdUserAndAvailable(int idUser, string searcher)
        {
            List<Stock> stocks = new List<Stock>();

            if (string.IsNullOrEmpty(searcher))
            {
                List<int> stocksId = (from sa in foodForAllContext.StockAvailable
                                      join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                                      join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                      where s.IdUser == idUser
                                      select sa.IdStock).ToList();

                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where stocksId.Contains(s.Id)
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }
            else
            {
                List<int> stocksId = (from sa in foodForAllContext.StockAvailable
                                      join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                                      join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                      where s.IdUser == idUser && p.Name.ToLower().Contains(searcher.ToLower())
                                      select sa.IdStock).ToList();

                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where stocksId.Contains(s.Id)
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }

            return stocks;
        }

        public List<Stock> findAllAvailable(string searcher)
        {
            List<Stock> stocks = new List<Stock>();

            if (string.IsNullOrEmpty(searcher))
            {
                List<int> stocksId = (from sa in foodForAllContext.StockAvailable
                                      join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                                      join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                      select sa.IdStock).ToList();

                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where stocksId.Contains(s.Id) && u.Status == true
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }
            else
            {
                List<int> stocksId = (from sa in foodForAllContext.StockAvailable
                                      join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                                      join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                      where p.Name.ToLower().Contains(searcher.ToLower())
                                      select sa.IdStock).ToList();

                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where stocksId.Contains(s.Id) && u.Status == true
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }

            return stocks;
        }

        public List<Stock> findByIdUserAndFilterDynamic(int idUser, string searcher, string property, string condition, string value)
        {
            List<Stock> stocks = new List<Stock>();

            Type type = typeof(Stock);
            PropertyInfo propertyInfo = type.GetProperty(property);

            if (string.IsNullOrEmpty(searcher))
            {
                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where s.IdUser == idUser && systemService.comparator(condition, propertyInfo.GetValue(s), value)
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }
            else
            {
                stocks = (from s in foodForAllContext.Stock
                          join p in foodForAllContext.Product on s.IdProduct equals p.Id
                          join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                          join u in foodForAllContext.User on s.IdUser equals u.Id
                          where s.IdUser == idUser && p.Name.ToLower().Contains(searcher.ToLower()) && systemService.comparator(condition, propertyInfo.GetValue(s), value)
                          orderby p.Name
                          select new Stock()
                          {
                              Star = s.Star,
                              DateOfAdmission = s.DateOfAdmission,
                              ExpirationDate = s.ExpirationDate,
                              Id = s.Id,
                              IdProduct = s.IdProduct,
                              IdUser = s.IdUser,
                              IsAvailable = s.IsAvailable,
                              Quantity = s.Quantity,
                              Status = s.Status,
                              UpdateDate = s.UpdateDate,
                              Observation = s.Observation,
                              IdUserNavigation = u,
                              IdProductNavigation = new Product()
                              {
                                  Description = p.Description,
                                  Id = p.Id,
                                  IdProductType = p.IdProductType,
                                  Name = p.Name,
                                  ReferenceImage = p.ReferenceImage,
                                  Status = p.Status,
                                  IdProductTypeNavigation = pt
                              }
                          }).ToList();
            }

            return stocks;
        }

        public Stock findById(int id)
        {
            Stock stock = (from s in foodForAllContext.Stock
                           join p in foodForAllContext.Product on s.IdProduct equals p.Id
                           join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                           join u in foodForAllContext.User on s.IdUser equals u.Id
                           where s.Id == id
                           select new Stock()
                           {
                               Star = s.Star,
                               DateOfAdmission = s.DateOfAdmission,
                               ExpirationDate = s.ExpirationDate,
                               Id = s.Id,
                               IdProduct = s.IdProduct,
                               IdUser = s.IdUser,
                               IsAvailable = s.IsAvailable,
                               Quantity = s.Quantity,
                               Status = s.Status,
                               UpdateDate = s.UpdateDate,
                               Observation = s.Observation,
                               IdUserNavigation = u,
                               IdProductNavigation = new Product()
                               {
                                   Description = p.Description,
                                   Id = p.Id,
                                   IdProductType = p.IdProductType,
                                   Name = p.Name,
                                   ReferenceImage = p.ReferenceImage,
                                   Status = p.Status,
                                   IdProductTypeNavigation = pt
                               }
                           }).FirstOrDefault();

            return stock;
        }

        public Stock findByIdAndAvailable(int id)
        {
            Stock stock = (from sa in foodForAllContext.StockAvailable
                           join s in foodForAllContext.Stock on sa.IdStock equals s.Id
                           where s.Id == id
                           select s).FirstOrDefault();

            return stock;
        }

        public Stock findByIdUserAndIdProduct(int idUser, int idProduct)
        {
            Stock stock = (from s in foodForAllContext.Stock where s.IdUser == idUser && s.IdProduct == idProduct select s).FirstOrDefault();

            return stock;
        }

        public Stock create(Stock stock)
        {
            stock.Status = true;
            stock.DateOfAdmission = DateTime.Now;

            foodForAllContext.Stock.Add(stock);
            foodForAllContext.SaveChanges();

            return stock;
        }

        public Stock updateById(Stock stock)
        {
            Stock stockExisting = (from s in foodForAllContext.Stock where s.Id == stock.Id select s).FirstOrDefault();

            stockExisting.Quantity = stock.Quantity;
            stockExisting.IsAvailable = stock.IsAvailable;
            stockExisting.ExpirationDate = stock.ExpirationDate;
            stockExisting.UpdateDate = DateTime.Now;
            stockExisting.Observation = stock.Observation;

            foodForAllContext.SaveChanges();

            return stockExisting;
        }

        public Stock updateStarById(int id, int star)
        {
            Stock stockExisting = (from s in foodForAllContext.Stock where s.Id == id select s).FirstOrDefault();

            stockExisting.Star = star;

            foodForAllContext.SaveChanges();

            return stockExisting;
        }

        public Stock updateStatusById(Stock stock)
        {
            Stock stockExisting = (from s in foodForAllContext.Stock where s.Id == stock.Id select s).FirstOrDefault();

            stockExisting.Status = stock.Status;
            stockExisting.UpdateDate = DateTime.Now;

            foodForAllContext.SaveChanges();

            return stockExisting;
        }
    }
}
