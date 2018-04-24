using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public enum ConversationMemberStatus
    {
        None=1,
        Admin=2,
        Active=3,
        Blocked=4,
        ReadOnly=5,
        LeftConversation=6
    }
}
