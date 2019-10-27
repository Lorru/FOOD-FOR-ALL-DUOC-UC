using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class Institution
    {
        public Institution()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Rut { get; set; }
        public string Activity { get; set; }
        public string Address { get; set; }
        public string Commune { get; set; }
        public int? Phone { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
