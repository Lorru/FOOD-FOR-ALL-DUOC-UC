using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class StockReceivedService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public StockReceivedService()
        {

        }

        public StockReceived create(StockReceived stockReceived)
        {

            foodForAllContext.StockReceived.Add(stockReceived);
            foodForAllContext.SaveChanges();

            Stock stock = (from s in foodForAllContext.Stock where s.Id == stockReceived.IdStock select s).FirstOrDefault();

            if (stock != null)
            {
                int quantity = stock.Quantity - stockReceived.Quantity;
                quantity = quantity < 0 ? 0 : quantity;

                stock.Quantity = quantity;
                stock.IsAvailable = quantity == 0 ? false : true;
                stock.UpdateDate = DateTime.Now;

                foodForAllContext.SaveChanges();

                if (!stock.IsAvailable)
                {
                    StockAvailable stockAvailable = (from sa in foodForAllContext.StockAvailable where sa.IdStock == stock.Id select sa).FirstOrDefault();
                    foodForAllContext.StockAvailable.Remove(stockAvailable);

                    foodForAllContext.SaveChanges();
                }
            }

            return stockReceived;
        }

        public StockReceived findByIdUserBeneficiaryAndIdStockAndDate(int idUserBeneficiary, int idStock, DateTime date)
        {
            StockReceived stockReceived = (from sr in foodForAllContext.StockReceived where sr.IdUserBeneficiary == idUserBeneficiary && sr.IdStock == idStock && sr.Date == date select sr).FirstOrDefault();

            return stockReceived;
        }

        public StockReceived updateQuantityById(StockReceived stockReceived)
        {
            StockReceived stockReceivedExisting = (from sr in foodForAllContext.StockReceived where sr.Id == stockReceived.Id select sr).FirstOrDefault();

            stockReceivedExisting.Quantity = stockReceivedExisting.Quantity + stockReceived.Quantity;

            foodForAllContext.SaveChanges();

            Stock stock = (from s in foodForAllContext.Stock where s.Id == stockReceived.IdStock select s).FirstOrDefault();

            if (stock != null)
            {
                int quantity = stock.Quantity - stockReceived.Quantity;
                quantity = quantity < 0 ? 0 : quantity;

                stock.Quantity = quantity;
                stock.IsAvailable = quantity == 0 ? false : true;
                stock.UpdateDate = DateTime.Now;

                foodForAllContext.SaveChanges();

                if (!stock.IsAvailable)
                {
                    StockAvailable stockAvailable = (from sa in foodForAllContext.StockAvailable where sa.IdStock == stock.Id select sa).FirstOrDefault();
                    foodForAllContext.StockAvailable.Remove(stockAvailable);

                    foodForAllContext.SaveChanges();
                }
            }

            return stockReceivedExisting;
        }

        public List<StockReceived> findByIdUserBeneficiary(int idUserBeneficiary, string searcher)
        {
            List<StockReceived> stockReceiveds = new List<StockReceived>();

            if (string.IsNullOrEmpty(searcher))
            {
                stockReceiveds = (from sr in foodForAllContext.StockReceived
                                  join s in foodForAllContext.Stock on sr.IdStock equals s.Id
                                  join u in foodForAllContext.User on s.IdUser equals u.Id
                                  join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                  join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                                  where sr.IdUserBeneficiary == idUserBeneficiary
                                  select new StockReceived()
                                  {
                                      IdStock = sr.IdStock,
                                      Date = sr.Date,
                                      Id = sr.Id,
                                      IdUserBeneficiary = sr.IdUserBeneficiary,
                                      Quantity = sr.Quantity,
                                      IdStockNavigation = new Stock()
                                      {
                                          Id = s.Id,
                                          DateOfAdmission = s.DateOfAdmission,
                                          ExpirationDate = s.ExpirationDate,
                                          IdProduct = s.IdProduct,
                                          IdUser = s.IdUser,
                                          IsAvailable = s.IsAvailable,
                                          Observation = s.Observation,
                                          Quantity = s.Quantity,
                                          Status = s.Status,
                                          UpdateDate = s.UpdateDate,
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
                                      }
                                  }).ToList();
            }
            else
            {
                stockReceiveds = (from sr in foodForAllContext.StockReceived
                                  join s in foodForAllContext.Stock on sr.IdStock equals s.Id
                                  join u in foodForAllContext.User on s.IdUser equals u.Id
                                  join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                  join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                                  where sr.IdUserBeneficiary == idUserBeneficiary && p.Name.ToLower().Contains(searcher.ToLower())
                                  select new StockReceived()
                                  {
                                      IdStock = sr.IdStock,
                                      Date = sr.Date,
                                      Id = sr.Id,
                                      IdUserBeneficiary = sr.IdUserBeneficiary,
                                      Quantity = sr.Quantity,
                                      IdStockNavigation = new Stock()
                                      {
                                          Id = s.Id,
                                          DateOfAdmission = s.DateOfAdmission,
                                          ExpirationDate = s.ExpirationDate,
                                          IdProduct = s.IdProduct,
                                          IdUser = s.IdUser,
                                          IsAvailable = s.IsAvailable,
                                          Observation = s.Observation,
                                          Quantity = s.Quantity,
                                          Status = s.Status,
                                          UpdateDate = s.UpdateDate,
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
                                      }
                                  }).ToList();

            }

            return stockReceiveds;
        }

        public List<StockReceived> findByIdStock(int idStock)
        {

            List<StockReceived> stockReceivedsDistinct = new List<StockReceived>();

            List<StockReceived> stockReceiveds = (from sr in foodForAllContext.StockReceived
                                                  join u in foodForAllContext.User on sr.IdUserBeneficiary equals u.Id
                                                  where sr.IdStock == idStock
                                                  select new StockReceived()
                                                  {
                                                      Date = sr.Date,
                                                      Id = sr.Id,
                                                      IdStock = sr.IdStock,
                                                      IdUserBeneficiary = sr.IdUserBeneficiary,
                                                      Quantity = sr.Quantity,
                                                      IdUserBeneficiaryNavigation = u
                                                  }).ToList();


            foreach (StockReceived stockReceived in stockReceiveds)
            {
                if (stockReceivedsDistinct.Where(sr => sr.IdUserBeneficiary == stockReceived.IdUserBeneficiary).FirstOrDefault() == null)
                {
                    stockReceivedsDistinct.Add(stockReceived);
                }
            }

            return stockReceivedsDistinct;
        }

        public StockReceived findById(int id)
        {
            StockReceived stockReceived = (from sr in foodForAllContext.StockReceived
                                           join s in foodForAllContext.Stock on sr.IdStock equals s.Id
                                           join u in foodForAllContext.User on s.IdUser equals u.Id
                                           join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                           join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                                           where sr.Id == id
                                           select new StockReceived()
                                           {
                                               IdStock = sr.IdStock,
                                               Date = sr.Date,
                                               Id = sr.Id,
                                               IdUserBeneficiary = sr.IdUserBeneficiary,
                                               Quantity = sr.Quantity,
                                               IdStockNavigation = new Stock()
                                               {
                                                   Id = s.Id,
                                                   DateOfAdmission = s.DateOfAdmission,
                                                   ExpirationDate = s.ExpirationDate,
                                                   IdProduct = s.IdProduct,
                                                   IdUser = s.IdUser,
                                                   IsAvailable = s.IsAvailable,
                                                   Observation = s.Observation,
                                                   Quantity = s.Quantity,
                                                   Status = s.Status,
                                                   UpdateDate = s.UpdateDate,
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
                                               }
                                           }).FirstOrDefault();

            return stockReceived;
        }
    }
}
