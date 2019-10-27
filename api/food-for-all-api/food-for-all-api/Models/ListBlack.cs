using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class ListBlack
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int? Phone { get; set; }
        public string Email { get; set; }
        public string OneSignalPlayerId { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
