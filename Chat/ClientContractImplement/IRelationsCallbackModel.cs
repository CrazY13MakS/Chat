using ContractClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientContractImplement
{
    public interface IRelationsCallbackModel
    {
        Collection<User> Friends { get; }
        Collection<User> FriendshipNotAllowed { get; }
        Collection<User> FriendshipRequestSend { get; }
        Collection<User> FriendshipRequestReceive { get; }


    }
}
