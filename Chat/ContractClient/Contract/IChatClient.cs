using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contract
{
    [ServiceContract]
   public interface IChatClient
    {
        void IncomingMessage(String body, String login, String conversationId, DateTime sendingtime);
        void FriendshipRequest(User user);
        void NetworkStatusChanged(String login, NetworkStatus status);
        void DeleteFrienship(String login);
        void BlockedByPartner(String login);
        void AddingToConversation(Conversation conversation);
    }
}
