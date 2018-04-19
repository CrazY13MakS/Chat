using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contracts
{
    [ServiceContract(CallbackContract = typeof(IChatClient), ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Required)]
    public interface IChatService
    {
        [OperationContract(IsInitiating = true)]
        bool Authentication(String token);

        [OperationContract(IsInitiating = false, IsTerminating = true)]
        bool Disconnect();

        [OperationContract(IsInitiating = false)]
        long SendMessage(String body, long conversationId);//-1 - не доставлено, >0 - номер сообщения в бд



        [OperationContract(IsInitiating =false)]
        List<ConversationReply> UpdateConversations(DateTime lastUpdated);

        /// <summary>
        /// Remove token
        /// </summary>
        /// <returns>Operation success</returns>
        [OperationContract]
        bool LogOut();

    }
}
