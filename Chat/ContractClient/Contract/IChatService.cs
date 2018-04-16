using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contract
{
    [ServiceContract(CallbackContract = typeof(IChatClient), ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Required)]
    public interface IChatService
    {
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        bool Authentication(String email, String password);

        [OperationContract(IsInitiating = true, IsTerminating = false)]
        bool AuthenticationWithToken(String token);

        [OperationContract(IsInitiating = false, IsTerminating = true)]
        bool Disconnect();

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        long SendMessage(String body, long conversationId);//-1 - не доставлено, >0 - номер сообщения в бд

        /// <summary>
        /// Запрос на добаление в друзья
        /// </summary>
        /// <param name="body">Текст при отправке запроса</param>
        /// <param name="userLogin">Уникальный логин аккаунта для отправки запроса</param>
        /// <returns></returns>
        [OperationContract(IsInitiating = false, IsTerminating = false)]        /// >0  - Id вашего чата
        User FriendshipRequest(String body, String userLogin);                  /// -1  - запрос уже отправлен
                                                                                /// -2 - вы в черном списке
        

    }
}
