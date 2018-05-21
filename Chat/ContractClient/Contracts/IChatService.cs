using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contracts
{
    [ServiceContract(CallbackContract = typeof(IChatCallback), ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Required)]
    public interface IChatService
    {
        /// <summary>
        /// Авторизация в системе с помощью токена
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [OperationContract(IsInitiating = true)]
        OperationResult<UserExt> Authentication(String token);

        /// <summary>
        /// Отключение от сервиса
        /// </summary>
        /// <returns></returns>
        [OperationContract(IsInitiating = false, IsTerminating = true)]
        OperationResult<bool> Disconnect();

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="body"></param>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        [OperationContract(IsInitiating = false)]
        OperationResult<bool> SendMessage(String body, long conversationId);//-1 - не доставлено, >0 - номер сообщения в бд


        ///// <summary>
        ///// Получить все сообщения с указанной даты
        ///// </summary>
        ///// <param name="lastSync"></param>
        ///// <returns></returns>
        //[OperationContract(IsInitiating =false)]
        //OperationResult<List<ConversationReply>> UpdateAllConversationsSinceDate(DateTime lastSync);



        ///// <summary>
        ///// Поулчить все сообщения
        ///// </summary>
        ///// <returns></returns>
        //[OperationContract(IsInitiating = false)]
        //OperationResult<List<ConversationReply>> UpdateAllConversations();

        ///// <summary>
        ///// Измениить статус сообщения
        ///// </summary>
        ///// <returns></returns>
        //[OperationContract(IsInitiating =false)]
        //OperationResult<bool> ChangeConversationReplyStatus();




        /// <summary>
        /// Remove token
        /// </summary>
        /// <returns>Operation success</returns>
        [OperationContract(IsInitiating =false,IsTerminating =false)]
        OperationResult<bool> LogOut();





        [OperationContract(IsInitiating = false)]
        OperationResult<List<Conversation>> GetConversations();



        [OperationContract(IsInitiating = false)]
        OperationResult<List<ConversationReply>> GetMessages(long conversationId);


        [OperationContract(IsInitiating = false)]
        OperationResult<Conversation> CreateDialog(String Login);

    }
}
