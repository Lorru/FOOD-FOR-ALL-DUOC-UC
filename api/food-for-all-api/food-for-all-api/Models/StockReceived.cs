using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class StockReceived
    {
        public int Id { get; set; }
        public int IdStock { get; set; }
        public int IdUserBeneficiary { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        public virtual Stock IdStockNavigation { get; set; }
        public virtual User IdUserBeneficiaryNavigation { get; set; }
    }
}
