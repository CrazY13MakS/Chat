using ContractClient;
using ContractClient.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientContractImplement
{
    public class ChatCustomerCallbackService : IChatCallback
    {
        public void AddingToConversation(Conversation conversation, string authorLogin)
        {
            int a = 5;
           // throw new NotImplementedException();
        }

        public void ConversationMemberStatusChanged(ConversationMemberStatus status, string authorLogin)
        {
            int a = 5;
           // throw new NotImplementedException();
        }

        public void IncomingMessage(ConversationReply reply)
        {
            int a = 5;
           // throw new NotImplementedException();
        }
    }
}
