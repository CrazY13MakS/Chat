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
        void FriendshipRequest(User user);
        void UserNetworkStatusChanged(String login, NetworkStatus status);
        void DeleteFriendship(String login);
        void BlockedByPartner(String login);
        void AddingToConversation(Conversation conversation);
    }
}
