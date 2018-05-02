using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractClient;

namespace ClientContractImplement
{
    public class AccountRelationsCallback : ContractClient.Contracts.IRelationsCallback
    {
        
        public AccountRelationsCallback()
        {
            
        }
        public void ChangeRelationType(string login, RelationStatus relationStatus)
        {
            throw new NotImplementedException();
        }

        public void FriendshipRequest(User user)
        {
            throw new NotImplementedException();
        }

        public void UserNetworkStatusChanged(string login, NetworkStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
