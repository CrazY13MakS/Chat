using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    [Serializable]
    public enum RelationStatus
    {
        None ,
        Friendship ,
        FriendshipRequestSent ,
        FrienshipRequestRecive ,
        BlockedByMe ,
        BlockedByPartner ,
        BlockedBoth 
    }
}
