using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class Product
    {
        public Product()
        {
            Stock = new HashSet<Stock>();
        }

        public int Id { get; set; }
        public int IdProductType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReferenceImage { get; set; }
        public bool Status { get; set; }

        public virtual ProductType IdProductTypeNavigation { get; set; }
        public virtual ICollection<Stock> Stock { get; set; }
    }
}
