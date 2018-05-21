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
        public OperationResult<UserExt> Authentication(string token)
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> Disconnect()
        {
            throw new NotImplementedException();
        }

        public OperationResult<List<Conversation>> GetConversations()
        {
            throw new NotImplementedException();
        }

        public OperationResult<List<ConversationReply>> GetMessages(long conversationId)
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> LogOut()
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> SendMessage(string body, long conversationId)
        {
            throw new NotImplementedException();
        }
    }
}
