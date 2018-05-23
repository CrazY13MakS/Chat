using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contracts
{
    [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Required)]
    public interface IChatCallback
    {
        [OperationContract]

        void IncomingMessage(ConversationReply reply);
        [OperationContract]

        void AddingToConversation(Conversation conversation, String authorLogin);
        [OperationContract]

        void ConversationMemberStatusChanged(ConversationMemberStatus status, String authorLogin);
    }
}
