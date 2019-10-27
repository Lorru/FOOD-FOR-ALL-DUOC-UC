using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class ProductService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private StockService stockService = new StockService();

        public ProductService()
        {

        }

        public List<Product> findAll(string searcher)
        {
            List<Product> products = new List<Product>();

            if (string.IsNullOrEmpty(searcher))
            {
                products = (from p in foodForAllContext.Product
                            join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                            where p.Status == true
                            orderby p.Name
                            select new Product()
                            {
                                Description = p.Description,
                                Id = p.Id,
                                IdProductType = p.IdProductType,
                                Name = p.Name,
                                ReferenceImage = p.ReferenceImage,
                                Status = p.Status,
                                IdProductTypeNavigation = pt
                            }).ToList();
            }
            else
            {
                products = (from p in foodForAllContext.Product
                            join pt in foodForAllContext.ProductType on p.IdProductType equals pt.Id
                            where p.Status == true && p.Name.ToLower().Contains(searcher.ToLower())
                            orderby p.Name
                            select new Product()
                            {
                                Description = p.Description,
                                Id = p.Id,
                                IdProductType = p.IdProductType,
                                Name = p.Name,
                                ReferenceImage = p.ReferenceImage,
                                Status = p.Status,
                                IdProductTypeNavigation = pt
                            }).ToList();
            }

            return products;
        }

        public Product findById(int id)
        {
            Product product = (from p in foodForAllContext.Product where p.Id == id select p).FirstOrDefault();

            return product;
        }

        public Product findByIdUserBeneficiaryAndLessReceived(int idUserBeneficiary)
        {
            Product product = null;

            List<Group> groups = (from sr in foodForAllContext.StockReceived
                                  where sr.IdUserBeneficiary == idUserBeneficiary
                                  group sr by sr.IdStock into s
                                  select new Group()
                                  {
                                      Key = s.Key,
                                      Count = s.Sum(sr => sr.Quantity)
                                  }).ToList();

            Group group = groups.OrderBy(g => g.Count).FirstOrDefault();

            if (group != null)
            {
                Stock stock = stockService.findById(Convert.ToInt32(group.Key));

                product = stock != null ? findById(stock.IdProduct) : null;
            }

            return product;
        }

        public Product findByIdUserBeneficiaryAndMoreReceived(int idUserBeneficiary)
        {
            Product product = null;

            List<Group> groups = (from sr in foodForAllContext.StockReceived
                                  where sr.IdUserBeneficiary == idUserBeneficiary
                                  group sr by sr.IdStock into s
                                  select new Group()
                                  {
                                      Key = s.Key,
                                      Count = s.Sum(sr => sr.Quantity)
                                  }).ToList();

            Group group = groups.OrderByDescending(g => g.Count).FirstOrDefault();

            if (group != null)
            {
                Stock stock = stockService.findById(Convert.ToInt32(group.Key));

                product = stock != null ? findById(stock.IdProduct) : null;
            }

            return product;
        }

        public Product findByIdUserBeneficiaryAndLastReceived(int idUserBeneficiary)
        {
            Product product = null;

            StockReceived stockReceived = (from sr in foodForAllContext.StockReceived where sr.IdUserBeneficiary == idUserBeneficiary select sr).OrderByDescending(sr => sr.Id).FirstOrDefault();

            if (stockReceived != null)
            {
                Stock stock = stockService.findById(stockReceived.IdStock);

                product = stock != null ? findById(stock.IdProduct) : null;
            }

            return product;
        }

        public Product findByIdUserDonorAndLastStock(int idUserDonor)
        {
            Stock stock = (from s in foodForAllContext.Stock where s.IdUser == idUserDonor select s).OrderByDescending(s => s.Id).FirstOrDefault();

            Product product = stock != null ? (from p in foodForAllContext.Product where p.Id == stock.IdProduct select p).FirstOrDefault() : null;

            return product;
        }

        public Product findByIdUserDonorAndLessQuantityStock(int idUserDonor)
        {
            List<Group> groups = (from s in foodForAllContext.Stock
                                  where s.IdUser == idUserDonor
                                  group s by s.IdProduct into s
                                  select new Group()
                                  {
                                      Key = s.Key,
                                      Count = s.Sum(sr => sr.Quantity)
                                  }).ToList();

            Group group = groups.OrderBy(g => g.Count).FirstOrDefault();

            Product product = group != null ? (from p in foodForAllContext.Product where p.Id == Convert.ToInt32(@group.Key) select p).FirstOrDefault() : null;

            return product;
        }
    }
}
