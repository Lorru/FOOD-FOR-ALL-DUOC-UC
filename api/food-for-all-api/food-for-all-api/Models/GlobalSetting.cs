using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class GlobalSetting
    {
        public int Id { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
        public bool Status { get; set; }
    }
}
