using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class UserType
    {
        public UserType()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
