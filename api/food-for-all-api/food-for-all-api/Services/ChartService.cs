using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class ChartService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();
        private UserService userService = new UserService();

        public ChartService()
        {

        }

        public List<Chart> findByIdUserAndChartDoughnut(int idUser)
        {
            User user = userService.findById(idUser);

            List<Chart> charts = new List<Chart>();

            if (user.IdUserType == 2)
            {
                List<Group> groups = (from s in foodForAllContext.Stock
                                      where s.IdUser == idUser & s.Status == true
                                      group s by s.IdProduct into sp
                                      select new Group()
                                      {
                                          Key = sp.Key,
                                          Count = sp.Sum(s => s.Quantity)
                                      }).ToList();

                foreach (Group group in groups)
                {
                    Product product = (from p in foodForAllContext.Product where p.Id == Convert.ToInt32(@group.Key) select p).FirstOrDefault();

                    Chart chart = new Chart();
                    chart.Data = group.Count;
                    chart.Label = product.Name;

                    charts.Add(chart);
                }
            }
            else if(user.IdUserType == 3)
            {
                List<Group> groups = (from sr in foodForAllContext.StockReceived
                                      where sr.IdUserBeneficiary == idUser
                                      group sr by sr.IdStock into sp
                                      select new Group()
                                      {
                                          Key = sp.Key,
                                          Count = sp.Sum(sr => sr.Quantity)
                                      }).ToList();

                foreach (Group group in groups)
                {
                    Product product = (from s in foodForAllContext.Stock join p in foodForAllContext.Product on s.IdProduct equals p.Id where s.Id == Convert.ToInt32(@group.Key) select p).FirstOrDefault();

                    Chart chart = new Chart();
                    chart.Data = group.Count;
                    chart.Label = product.Name;

                    charts.Add(chart);
                }
            }

            return charts;
        }

        public List<Chart> findByIdUserAndChartDoughnutDonor(int idUser)
        {
            List<Chart> charts = new List<Chart>();

            List<Group> groups = (from sr in foodForAllContext.StockReceived
                                  join s in foodForAllContext.Stock on sr.IdStock equals s.Id
                                  where s.IdUser == idUser
                                  group sr by sr.IdStock into sp
                                  select new Group()
                                  {
                                      Key = sp.Key,
                                      Count = sp.Sum(sr => sr.Quantity)
                                  }).ToList();

            foreach (Group group in groups)
            {
                Product product = (from s in foodForAllContext.Stock join p in foodForAllContext.Product on s.IdProduct equals p.Id where s.Id == Convert.ToInt32(@group.Key) select p).FirstOrDefault();

                Chart chart = new Chart();
                chart.Data = group.Count;
                chart.Label = product.Name;

                charts.Add(chart);
            }

            return charts;
        }

        public List<Chart> findAllChartDoughnut()
        {
            List<Chart> charts = new List<Chart>();

            List<Group> groups = (from sr in foodForAllContext.StockReceived
                                  join s in foodForAllContext.Stock on sr.IdStock equals s.Id
                                  group sr by sr.IdStock into sp
                                  select new Group()
                                  {
                                      Key = sp.Key,
                                      Count = sp.Sum(sr => sr.Quantity)
                                  }).ToList();

            foreach (Group group in groups)
            {
                Product product = (from s in foodForAllContext.Stock join p in foodForAllContext.Product on s.IdProduct equals p.Id where s.Id == Convert.ToInt32(@group.Key) select p).FirstOrDefault();

                Chart chart = new Chart();
                chart.Data = group.Count;
                chart.Label = product.Name;

                charts.Add(chart);
            }

            return charts;
        }

        public List<Chart> findByIdUserAndChartBar(int idUser)
        {
            User user = userService.findById(idUser);

            List<Chart> charts = new List<Chart>();

            if (user.IdUserType == 2)
            {
                List<Group> groups = (from s in foodForAllContext.Stock
                                      where s.IdUser == idUser & s.Status == true
                                      group s by Convert.ToDateTime(s.DateOfAdmission).ToString("yyyy-MM-dd") into sp
                                      select new Group()
                                      {
                                          Key = sp.Key,
                                      }).ToList();

                foreach (Group group in groups)
                {
                    Chart chart = new Chart();
                    chart.Data = (from s in foodForAllContext.Stock where Convert.ToDateTime(s.DateOfAdmission).ToString("yyyy-MM-dd") == Convert.ToDateTime(@group.Key).ToString("yyyy-MM-dd") && s.IdUser == idUser select s).Sum(s => s.Quantity);
                    chart.Label = Convert.ToDateTime(group.Key).ToString("yyyy-MM-dd");

                    charts.Add(chart);
                }
            }
            else if (user.IdUserType == 3)
            {
                List<Group> groups = (from sr in foodForAllContext.StockReceived
                                      where sr.IdUserBeneficiary == idUser
                                      group sr by sr.Date into sp
                                      select new Group()
                                      {
                                          Key = sp.Key,
                                          Count = sp.Sum(sr => sr.Quantity)
                                      }).ToList();

                foreach (Group group in groups)
                {
                    Chart chart = new Chart();
                    chart.Data = group.Count;
                    chart.Label = Convert.ToDateTime(group.Key).ToString("yyyy-MM-dd");

                    charts.Add(chart);
                }
            }

            return charts;
        }
    }
}
