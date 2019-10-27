using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class StockComment
    {
        public int Id { get; set; }
        public int IdTypeMessage { get; set; }
        public int IdStock { get; set; }
        public int IdUser { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public virtual Stock IdStockNavigation { get; set; }
        public virtual TypeMessage IdTypeMessageNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
