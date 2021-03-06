﻿using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class StockImage
    {
        public int Id { get; set; }
        public int IdStock { get; set; }
        public string ReferenceImage { get; set; }

        public virtual Stock IdStockNavigation { get; set; }
    }
}
