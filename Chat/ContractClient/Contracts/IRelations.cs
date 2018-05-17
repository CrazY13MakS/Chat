using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace ContractClient.Contracts
{
    [ServiceContract(CallbackContract = typeof(IRelationsCallback), ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Required)]
    public interface IRelations
    {


        // TODO: Send user on Auth
        [OperationContract(IsInitiating = true)]
        OperationResult<bool> Authentication(String token);


        /// <summary>
        /// Запрос на добаление в друзья
        /// </summary>
        /// <param name="body">Текст при отправке запроса</param>
        /// <param name="userLogin">Уникальный логин аккаунта для отправки запроса</param>
        /// <returns></returns>
        [OperationContract(IsInitiating =false)]                                                      /// >0  - Id вашего чата
        OperationResult<User> FriendshipRequest(String body, String userLogin);                  /// -1  - запрос уже отправлен
                                                                                                 /// -2 - вы в черном списке
        [OperationContract(IsInitiating = false)]
        OperationResult<bool> ChangeNetworkStatus( NetworkStatus status);

        [OperationContract(IsInitiating = false)]
        OperationResult<List<User>> FindUsers( String param);

        [OperationContract(IsInitiating = false)]
        OperationResult<bool> UpdateProfile( UserExt user);

        [OperationContract(IsInitiating = false)]
        OperationResult<bool> ChangeRelationType(String login, RelationStatus status);

        //[OperationContract(IsInitiating = false)]
        //OperationResult<bool> FrienshipResponse( String login, bool isConfirmed);

        //[OperationContract(IsInitiating = false)]
        //OperationResult<bool> BlockUser(String login);

        //[OperationContract(IsInitiating = false)]
        //OperationResult<bool> UnBlockUser( String login);

        //[OperationContract(IsInitiating = false)]
        //OperationResult<List<User>> GetBlockedUsers();

        //[OperationContract(IsInitiating = false)]
        //OperationResult<List<User>> GetFriends();

        [OperationContract(IsInitiating = false)]
        OperationResult<List<User>> GetUsersByRelationStatus(RelationStatus status);
    }
}
