using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class Stock
    {
        public Stock()
        {
            CalificationStock = new HashSet<CalificationStock>();
            StockAvailable = new HashSet<StockAvailable>();
            StockComment = new HashSet<StockComment>();
            StockImage = new HashSet<StockImage>();
            StockReceived = new HashSet<StockReceived>();
        }

        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Observation { get; set; }
        public int? Star { get; set; }
        public bool Status { get; set; }

        public virtual Product IdProductNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
        public virtual ICollection<CalificationStock> CalificationStock { get; set; }
        public virtual ICollection<StockAvailable> StockAvailable { get; set; }
        public virtual ICollection<StockComment> StockComment { get; set; }
        public virtual ICollection<StockImage> StockImage { get; set; }
        public virtual ICollection<StockReceived> StockReceived { get; set; }
    }
}
