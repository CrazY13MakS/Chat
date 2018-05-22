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
            throw new NotImplementedException();
        }

        public void ConversationMemberStatusChanged(ConversationMemberStatus status, string authorLogin)
        {
            throw new NotImplementedException();
        }

        public void IncomingMessage(ConversationReply reply)
        {
            throw new NotImplementedException();
        }
    }
}
