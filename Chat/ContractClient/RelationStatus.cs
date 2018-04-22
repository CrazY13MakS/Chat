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
        None=1,
        Friendship=2,
        FriendshipRequestSent=3,
        FrienshipRequestRecive=4,
        BlockedByMe=5,
        BlockedByPartner=6
    }
}
