using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class Location
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public bool IsMain { get; set; }
        public bool Status { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
