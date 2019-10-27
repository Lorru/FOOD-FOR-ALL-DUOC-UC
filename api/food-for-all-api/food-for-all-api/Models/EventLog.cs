using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class EventLog
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int IdEventLogType { get; set; }
        public string Controller { get; set; }
        public string Method { get; set; }
        public string HttpMethod { get; set; }
        public string Host { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public virtual EventLogType IdEventLogTypeNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
