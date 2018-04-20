using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace ContractClient.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
   public interface IAccountUpdate
    {
        /// <summary>
        /// Запрос на добаление в друзья
        /// </summary>
        /// <param name="body">Текст при отправке запроса</param>
        /// <param name="userLogin">Уникальный логин аккаунта для отправки запроса</param>
        /// <returns></returns>
        [OperationContract]                                                      /// >0  - Id вашего чата
        User FriendshipRequest(String body, String userLogin);                  /// -1  - запрос уже отправлен
                                                                                /// -2 - вы в черном списке
        [OperationContract]
        bool ChangeNetworkStatus(String token, NetworkStatus status);

        [OperationContract]
        List<User> FindUsers(String token, String param);

        [OperationContract]
        String UpdateProfile(UserExt user);

        [OperationContract]
        bool FrienshipResponse(String login, bool isConfirmed);

        [OperationContract]
        bool BlockedUser(String login);

        [OperationContract]
        bool UnBlockedUser(String login);
    }
}
