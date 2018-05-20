using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contracts
{
    [ServiceContract]
   public interface IChatCallback
    {
        void IncomingMessage(String body, String login, String conversationId, DateTime sendingtime);
        void AddingToConversation(Conversation conversation);
    }
}
