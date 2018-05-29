using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contracts
{
    [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Required)]
    public interface IRelationsCallback
    {
        [OperationContract(IsOneWay = true)]
        void FriendshipRequest(User user);
        [OperationContract(IsOneWay = true)]
        void UserNetworkStatusChanged(String login, NetworkStatus status);
        [OperationContract(IsOneWay =true)]
        void ChangeRelationType(String login, RelationStatus relationStatus);
     //   [OperationContract]

    }
}
