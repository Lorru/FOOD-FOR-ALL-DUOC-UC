using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class Token
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Token1 { get; set; }
        public string Host { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
