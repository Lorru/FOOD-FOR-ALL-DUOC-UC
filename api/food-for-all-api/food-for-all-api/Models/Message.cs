using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int IdTypeMessage { get; set; }
        public int IdUserSend { get; set; }
        public int IdUserReceived { get; set; }
        public string Message1 { get; set; }
        public DateTime Date { get; set; }

        public virtual TypeMessage IdTypeMessageNavigation { get; set; }
        public virtual User IdUserReceivedNavigation { get; set; }
        public virtual User IdUserSendNavigation { get; set; }
    }
}
