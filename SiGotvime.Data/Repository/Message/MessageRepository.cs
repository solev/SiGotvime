using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGotvime.Data.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private FoodDatabase db;

        public MessageRepository(FoodDatabase _db)
        {
            db = _db;
        }

        public void CreateMessage(Models.Message message)
        {
            if(message.UserBy!=null)
            {
                message.UserBy = db.Users.FirstOrDefault(x => x.ID == message.UserBy.ID);
            }

            db.Messages.Add(message);
            db.SaveChanges();
        }
    }
}
