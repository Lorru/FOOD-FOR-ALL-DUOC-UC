using food_for_all_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace food_for_all_api.Services
{
    public class MessageService
    {
        private FoodForAllContext foodForAllContext = new FoodForAllContext();

        public MessageService()
        {

        }

        public Message create(Message message)
        {

            message.Date = DateTime.Now;

            foodForAllContext.Message.Add(message);
            foodForAllContext.SaveChanges();

            return message;
        }

        public Message findById(int id)
        {
            Message message = (from m in foodForAllContext.Message where m.Id == id select m).FirstOrDefault();

            return message;
        }

        public Message destroyById(int id)
        {
            Message message = (from m in foodForAllContext.Message where m.Id == id select m).FirstOrDefault();

            foodForAllContext.Message.Remove(message);
            foodForAllContext.SaveChanges();

            message = findById(id);

            return message;
        }

        public List<Message> findByIdUserSendAndIdUserReceived(int idUserSend, int idUserReceived, string searcher)
        {
            List<Message> messages = new List<Message>();

            if (string.IsNullOrEmpty(searcher))
            {
                messages = (from m in foodForAllContext.Message
                            join us in foodForAllContext.User on m.IdUserSend equals us.Id
                            join ur in foodForAllContext.User on m.IdUserReceived equals ur.Id
                            where (m.IdUserSend == idUserSend && m.IdUserReceived == idUserReceived) || (m.IdUserSend == idUserReceived && m.IdUserReceived == idUserSend)
                            select new Message()
                            {
                                Date = m.Date,
                                Id = m.Id,
                                IdTypeMessage = m.IdTypeMessage,
                                IdUserReceived = m.IdUserReceived,
                                IdUserSend = m.IdUserSend,
                                Message1 = m.Message1,
                                IdUserReceivedNavigation = ur,
                                IdUserSendNavigation = us
                            }).ToList();
            }
            else
            {
                messages = (from m in foodForAllContext.Message
                            join us in foodForAllContext.User on m.IdUserSend equals us.Id
                            join ur in foodForAllContext.User on m.IdUserReceived equals ur.Id
                            where (m.IdUserSend == idUserSend && m.IdUserReceived == idUserReceived) || (m.IdUserSend == idUserReceived && m.IdUserReceived == idUserSend) && m.Message1.ToLower().Contains(searcher.ToLower())
                            select new Message()
                            {
                                Date = m.Date,
                                Id = m.Id,
                                IdTypeMessage = m.IdTypeMessage,
                                IdUserReceived = m.IdUserReceived,
                                IdUserSend = m.IdUserSend,
                                Message1 = m.Message1,
                                IdUserReceivedNavigation = ur,
                                IdUserSendNavigation = us
                            }).ToList();
            }

            return messages;
        }

        public List<Message> findByIdUserLast(int idUser)
        {
            List<Message> messages = new List<Message>();

            List<int> idUserSends = (from m in foodForAllContext.Message where m.IdUserReceived == idUser select m.IdUserSend).Distinct().ToList();
            List<int> idUserReceiveds = (from m in foodForAllContext.Message where m.IdUserSend == idUser select m.IdUserReceived).Distinct().ToList();

            List<int> idUsers = new List<int>();

            idUsers = idUsers.Union(idUserSends).ToList();
            idUsers = idUsers.Union(idUserReceiveds).ToList();
            idUsers = idUsers.Distinct().ToList();

            foreach (int idUserSendOrReceived in idUsers)
            {
                Message message = (from m in foodForAllContext.Message
                                   join us in foodForAllContext.User on m.IdUserSend equals us.Id
                                   join ur in foodForAllContext.User on m.IdUserReceived equals ur.Id
                                   where m.IdUserSend == idUserSendOrReceived || m.IdUserReceived == idUserSendOrReceived
                                   orderby m.Id descending
                                   select new Message()
                                   {
                                       Date = m.Date,
                                       Id = m.Id,
                                       IdTypeMessage = m.IdTypeMessage,
                                       IdUserReceived = m.IdUserReceived,
                                       IdUserSend = m.IdUserSend,
                                       Message1 = m.Message1,
                                       IdUserReceivedNavigation = ur,
                                       IdUserSendNavigation = us
                                   }).FirstOrDefault();

                if (message != null)
                {
                    messages.Add(message);
                }
            }

            return messages.OrderByDescending(m => m.Id).ToList();
        }
    }
}
