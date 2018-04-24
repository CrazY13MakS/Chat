using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractClient;
using ContractClient.Contracts;
namespace ChatServiceProvider
{
    public class ChatService : IChatService
    {
        public UserExt Authentication(string token)
        {
            throw new NotImplementedException();
        }

        public bool ChangeConversationReplyStatus()
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool LogOut()
        {
            throw new NotImplementedException();
        }

        public ConversationReplyStatus SendMessage(string body, long conversationId)
        {
            throw new NotImplementedException();
        }

        public List<ConversationReply> UpdateAllConversations()
        {
            throw new NotImplementedException();
        }

        public List<ConversationReply> UpdateAllConversationsSinceDate(DateTime lastSync)
        {
            throw new NotImplementedException();
        }
    }
}
