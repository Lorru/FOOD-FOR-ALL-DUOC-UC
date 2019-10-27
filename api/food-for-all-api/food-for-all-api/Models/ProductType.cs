using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Product = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReferenceImage { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
