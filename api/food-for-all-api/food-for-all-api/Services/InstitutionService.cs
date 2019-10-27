using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class InstitutionService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public InstitutionService()
        {

        }

        public List<Institution> findAll(string searcher)
        {
            List<Institution> institutions = new List<Institution>();

            if (string.IsNullOrEmpty(searcher))
            {
                institutions = (from i in foodForAllContext.Institution where i.Status == true select i).ToList();
            }
            else
            {
                institutions = (from i in foodForAllContext.Institution
                                where i.Status == true && (i.Name.ToLower().Contains(searcher.ToLower()) || i.Activity.ToLower().Contains(searcher.ToLower()))
                                select i).ToList();
            }

            return institutions;
        }
    }
}
