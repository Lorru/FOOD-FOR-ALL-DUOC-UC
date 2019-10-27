using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class Denounced
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdUserAccuser { get; set; }
        public string Reason { get; set; }

        public virtual User IdUserAccuserNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
