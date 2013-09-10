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
    //Prova commit 2
    public class LoggingOperationManager
    {
        public CategoryType getCategoryFromString(String category)
        {
            using (var log = new LoggingContext())
            {
              
            }

            return 0;
         
        }
        public List<Message> readAllMessagesHavingParameters(String messageText, String source, String category)
        {
            List<Message> messagesToBack = new List<Message>();
            using (var log = new LoggingContext())
            {
                //if (messageText.Equals(""))
                //{
                //    if (source.Equals(""))
                //    {
                //        if (category.Equals(""))
                //        {

                //        }
                //        else
                //        {

                //        }
                //    }
                //    else
                //    {

                //    }
                //}
                //else
                //{

                //}

            }

            return messagesToBack;

        }

        public List<Message> readAllMessagesBetweenDatetime(DateTime start, DateTime end)
        {
            List<Message> messagesToBack = new List<Message>();
            using (var log = new LoggingContext())
            {
                

            }
            return messagesToBack;

        }

        public void writeMessage(Message message)
        {
            MessageDAL md = messagetoMessageDAL(message);
           
            Console.WriteLine(md.timestamp);
 

            using (var log = new LoggingContext())
            {
                log.messages.Add(md);
                log.SaveChanges();
            }
        }
        
        public void writeAllMessages(List<Message> messages)
        {
            using (var log = new LoggingContext())
            {

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
    }
}

