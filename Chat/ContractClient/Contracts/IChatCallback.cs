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

        /// <summary>
        /// Входящее сообщение
        /// </summary>
        /// <param name="reply">Сообщение</param>
        [OperationContract]
        void IncomingMessage(ConversationReply reply);


        /// <summary>
        /// Добавление в беседу 
        /// </summary>
        /// <param name="conversation">Беседа</param>
        /// <param name="authorLogin">Кто добавил</param>
        [OperationContract]
        void AddingToConversation(Conversation conversation, String authorLogin);

        /// <summary>
        /// Изменение статуса участника беседы
        /// </summary>
        /// <param name="conversationId">Номер беседы</param>
        /// <param name="whoChangedLogin">Логин того кто изменил статус</param>
        /// <param name="whomHeChanged">Логин того кому изменен статус</param>
        /// <param name="status">Новый статус</param>
        [OperationContract]
        void ConversationMemberStatusChanged(long conversationId, ConversationMemberStatus status);
    }
}
