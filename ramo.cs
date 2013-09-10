using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Infrastructure.logging.datamodel.model;


namespace Infrastructure.logging.dal
{
    //Prova commit
    //Prova commit 4
    public class LoggingOperationManager
    {

        public List<Message> readAllMessages(Message parameters, Boolean isOr)
        {
            List<Message> messagesToBack = new List<Message>();

            using (var log = new LoggingContext())
            {
                //IQueryable<MessageDAL> result;
                //if (!isOr)
                //{
                //    result = log.messages.Where(m => (m.message == parameters.messaggio) && (m.source == parameters.sorgente) && (m.categoryDAL.CategoryId == (int)parameters.tipo.idTipo));
                //}
                //else
                //{
                //    result = log.messages.Where(m => (m.message == parameters.messaggio) || (m.source == parameters.sorgente) || (m.categoryDAL.CategoryId == (int)parameters.tipo.idTipo));
                //}
                //if (result != null)
                //{
                //    messagesToBack = messagesDALtoMessages(result.ToList());
                //}

                var query = from m in log.messages
                            join cat in log.categories
                            on m.TypeId equals cat.CategoryId
                            select new Message
                            {
                                exception = m.exception,
                                messaggio =  m.message,
                                id = m.MessageId,
                                sorgente = m.source,
                                timeStamp = m.timestamp,
                                tipo = new Category{ idTipo = (CategoryType)cat.CategoryId, tipo = cat.name}
                              
                            };
                messagesToBack = query.ToList();
            }


           
            return messagesToBack;
        }

        public void writeMessage(Message message)
        {
            MessageDAL md = messagetoMessageDAL(message);

            using (var log = new LoggingContext())
            {
                log.messages.Add(md);
                log.SaveChanges();
            }
        }



        // Mappa un oggetto di tipo MessageDAL (Infrastructure.logging.dal) su un oggetto Message(Infrastructure.logging.datamodel.model)
        public Message messageDALtoMessage(MessageDAL messageDAL)
        {
            Message messageToBack = new Message();
            messageToBack.id = messageDAL.MessageId;
            messageToBack.messaggio = messageDAL.message;
            messageToBack.sorgente = messageDAL.source;
            messageToBack.exception = messageDAL.exception;

            messageToBack.tipo = categoryDALtoCategory(messageDAL.categoryDAL);

            return messageToBack;
        }

        // Mappa un oggetto di tipo Message(Infrastructure.logging.datamodel.model) su un oggetto MessageDAL (Infrastructure.logging.dal)
        public MessageDAL messagetoMessageDAL(Message message)
        {
            MessageDAL messageDAL = new MessageDAL();
            messageDAL.MessageId = message.id;
            messageDAL.message = message.messaggio;
            messageDAL.source = message.sorgente;

            // la data di un nuovo messaggio di log è sempre settata al tempo corrente.
            messageDAL.timestamp = DateTime.Now;
            messageDAL.exception = message.exception;
            messageDAL.CategoryId = categoryToCategoryDAL(message.tipo).CategoryId;
            return messageDAL;
        }

        // Mappa un oggetto di tipo Category(Infrastructure.logging.datamodel.model) su un oggetto CategoryDAL (Infrastructure.logging.dal)
        public CategoryDAL categoryToCategoryDAL(Category category)
        {
            CategoryDAL categoryDALToBack = new CategoryDAL();

            using (var log = new LoggingContext())
            {
                var result = (log.categories.Where(cat => (cat.CategoryId == (int)category.idTipo)));

                if (result != null)
                {
                    categoryDALToBack.CategoryId = result.First().CategoryId;
                    categoryDALToBack.name = result.First().name;
                }
            }
            return categoryDALToBack;
        }

        // Mappa un oggetto di tipo CategoryDAL (Infrastructure.logging.dal) su un oggetto Category(Infrastructure.logging.datamodel.model)
        public Category categoryDALtoCategory(CategoryDAL categoryDAL)
        {
            Category categoryToBack = new Category();

            categoryToBack.idTipo = (CategoryType)categoryDAL.CategoryId;
            categoryToBack.tipo = categoryDAL.name;

            return categoryToBack;
        }

        public List<Message> messagesDALtoMessages(List<MessageDAL> messagesDAL)
        {
            List<Message> messagesToBack = new List<Message>();
            foreach (MessageDAL md in messagesDAL)
            {
                messagesToBack.Add(messageDALtoMessage(md));
            }

            return messagesToBack;
        }
    }
}
