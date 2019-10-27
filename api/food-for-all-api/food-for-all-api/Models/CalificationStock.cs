using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class CalificationStock
    {
        public int Id { get; set; }
        public int IdStock { get; set; }
        public int IdUserCalification { get; set; }
        public int Calification { get; set; }

        public virtual Stock IdStockNavigation { get; set; }
        public virtual User IdUserCalificationNavigation { get; set; }
    }
}
