using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class StockCommentService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public StockCommentService()
        {

        }

        public StockComment create(StockComment stockComment)
        {
            stockComment.Date = DateTime.Now;

            foodForAllContext.StockComment.Add(stockComment);
            foodForAllContext.SaveChanges();

            return stockComment;
        }

        public List<StockComment> findByIdStock(int idStock)
        {
            List<StockComment> stockComments = (from sc in foodForAllContext.StockComment
                                                join u in foodForAllContext.User on sc.IdUser equals u.Id
                                                where sc.IdStock == idStock
                                                orderby sc.Id descending
                                                select new StockComment()
                                                {
                                                    IdTypeMessage = sc.IdTypeMessage,
                                                    Date = sc.Date,
                                                    Comment = sc.Comment,
                                                    Id = sc.Id,
                                                    IdStock = sc.IdStock,
                                                    IdUser = sc.IdUser,
                                                    IdUserNavigation = u
                                                }).ToList();

            return stockComments;
        }

        public StockComment findById(int id)
        {
            StockComment stockComment = (from sc in foodForAllContext.StockComment where sc.Id == id select sc).FirstOrDefault();

            return stockComment;
        }

        public StockComment destroyById(int id)
        {
            StockComment stockComment = (from sc in foodForAllContext.StockComment where sc.Id == id select sc).FirstOrDefault();

            foodForAllContext.StockComment.Remove(stockComment);
            foodForAllContext.SaveChanges();

            stockComment = findById(id);

            return stockComment;
        }

        public StockComment findByIdUserBeneficiaryAndLastComment(int idUserBeneficiary)
        {
            StockComment stockComment = (from sc in foodForAllContext.StockComment
                                         join s in foodForAllContext.Stock on sc.IdStock equals s.Id
                                         join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                         join u in foodForAllContext.User on sc.IdUser equals u.Id
                                         where sc.IdUser == idUserBeneficiary
                                         select new StockComment()
                                         {
                                             IdTypeMessage = sc.IdTypeMessage,
                                             Comment = sc.Comment,
                                             Date = sc.Date,
                                             Id = sc.Id,
                                             IdStock = sc.IdStock,
                                             IdUser = sc.IdUser,
                                             IdUserNavigation = u,
                                             IdStockNavigation = new Stock()
                                             {
                                                 DateOfAdmission = s.DateOfAdmission,
                                                 ExpirationDate = s.ExpirationDate,
                                                 Id = s.Id,
                                                 IdProduct = s.IdProduct,
                                                 IdUser = s.IdUser,
                                                 IsAvailable = s.IsAvailable,
                                                 Observation = s.Observation,
                                                 Quantity = s.Quantity,
                                                 Status = s.Status,
                                                 UpdateDate = s.UpdateDate,
                                                 IdProductNavigation = p
                                             }
                                         }).OrderByDescending(sc => sc.Id).FirstOrDefault();

            return stockComment;
        }

        public StockComment findByIdUserDonorAndLastComment(int idUserDonor)
        {
            StockComment stockComment = (from sc in foodForAllContext.StockComment
                                         join s in foodForAllContext.Stock on sc.IdStock equals s.Id
                                         join p in foodForAllContext.Product on s.IdProduct equals p.Id
                                         join u in foodForAllContext.User on sc.IdUser equals u.Id
                                         where s.IdUser == idUserDonor
                                         select new StockComment()
                                         {
                                             IdTypeMessage = sc.IdTypeMessage,
                                             Comment = sc.Comment,
                                             Date = sc.Date,
                                             Id = sc.Id,
                                             IdStock = sc.IdStock,
                                             IdUser = sc.IdUser,
                                             IdUserNavigation = u,
                                             IdStockNavigation = new Stock()
                                             {
                                                 DateOfAdmission = s.DateOfAdmission,
                                                 ExpirationDate = s.ExpirationDate,
                                                 Id = s.Id,
                                                 IdProduct = s.IdProduct,
                                                 IdUser = s.IdUser,
                                                 IsAvailable = s.IsAvailable,
                                                 Observation = s.Observation,
                                                 Quantity = s.Quantity,
                                                 Status = s.Status,
                                                 UpdateDate = s.UpdateDate,
                                                 IdProductNavigation = p
                                             }
                                         }).OrderByDescending(sc => sc.Id).FirstOrDefault();

            return stockComment;
        }
    }
}
