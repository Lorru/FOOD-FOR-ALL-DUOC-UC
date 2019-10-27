using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class TypeMessage
    {
        public TypeMessage()
        {
            Message = new HashSet<Message>();
            StockComment = new HashSet<StockComment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<StockComment> StockComment { get; set; }
    }
}
