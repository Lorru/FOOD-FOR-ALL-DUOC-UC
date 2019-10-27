using System;
using System.Collections.Generic;

namespace food_for_all_api.Models
{
    public partial class User
    {
        public User()
        {
            CalificationStock = new HashSet<CalificationStock>();
            CalificationUserIdUserCalificationNavigation = new HashSet<CalificationUser>();
            CalificationUserIdUserNavigation = new HashSet<CalificationUser>();
            DenouncedIdUserAccuserNavigation = new HashSet<Denounced>();
            DenouncedIdUserNavigation = new HashSet<Denounced>();
            EventLog = new HashSet<EventLog>();
            ListBlack = new HashSet<ListBlack>();
            Location = new HashSet<Location>();
            MessageIdUserReceivedNavigation = new HashSet<Message>();
            MessageIdUserSendNavigation = new HashSet<Message>();
            Stock = new HashSet<Stock>();
            StockComment = new HashSet<StockComment>();
            StockReceived = new HashSet<StockReceived>();
            Token = new HashSet<Token>();
        }

        public int Id { get; set; }
        public int IdUserType { get; set; }
        public int? IdInstitution { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public string Photo { get; set; }
        public string OneSignalPlayerId { get; set; }
        public int? Star { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public bool IsWithFacebook { get; set; }
        public bool? OnLine { get; set; }
        public bool Status { get; set; }

        public virtual Institution IdInstitutionNavigation { get; set; }
        public virtual UserType IdUserTypeNavigation { get; set; }
        public virtual ICollection<CalificationStock> CalificationStock { get; set; }
        public virtual ICollection<CalificationUser> CalificationUserIdUserCalificationNavigation { get; set; }
        public virtual ICollection<CalificationUser> CalificationUserIdUserNavigation { get; set; }
        public virtual ICollection<Denounced> DenouncedIdUserAccuserNavigation { get; set; }
        public virtual ICollection<Denounced> DenouncedIdUserNavigation { get; set; }
        public virtual ICollection<EventLog> EventLog { get; set; }
        public virtual ICollection<ListBlack> ListBlack { get; set; }
        public virtual ICollection<Location> Location { get; set; }
        public virtual ICollection<Message> MessageIdUserReceivedNavigation { get; set; }
        public virtual ICollection<Message> MessageIdUserSendNavigation { get; set; }
        public virtual ICollection<Stock> Stock { get; set; }
        public virtual ICollection<StockComment> StockComment { get; set; }
        public virtual ICollection<StockReceived> StockReceived { get; set; }
        public virtual ICollection<Token> Token { get; set; }
    }
}
