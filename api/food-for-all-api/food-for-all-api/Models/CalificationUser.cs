using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class CalificationUser
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdUserCalification { get; set; }
        public int Calification { get; set; }

        public virtual User IdUserCalificationNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
