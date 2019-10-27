using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class StockAvailable
    {
        public int Id { get; set; }
        public int IdStock { get; set; }
        public DateTime DateOfAdmission { get; set; }

        public virtual Stock IdStockNavigation { get; set; }
    }
}
