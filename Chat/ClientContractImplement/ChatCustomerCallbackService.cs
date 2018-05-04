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
        public void AddingToConversation(Conversation conversation)
        {
            throw new NotImplementedException();
        }

        public void BlockedByPartner(string login)
        {
            throw new NotImplementedException();
        }

        public void DeleteFriendship(string login)
        {
            throw new NotImplementedException();
        }

        public void FriendshipRequest(User user)
        {
            throw new NotImplementedException();
        }

        public void IncomingMessage(string body, string login, string conversationId, DateTime sendingtime)
        {
            throw new NotImplementedException();
        }

        public void UserNetworkStatusChanged(string login, NetworkStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
