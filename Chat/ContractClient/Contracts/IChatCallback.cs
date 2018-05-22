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
        void IncomingMessage(ConversationReply reply);
        void AddingToConversation(Conversation conversation, String authorLogin);
        void ConversationMemberStatusChanged(ConversationMemberStatus status,String authorLogin);        
    }
}
