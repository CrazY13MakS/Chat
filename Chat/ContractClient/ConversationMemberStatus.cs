using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public enum ConversationMemberStatus
    {
        None,
        Admin,
        Active,
        Blocked,
        ReadOnly,
        LeftConversation
    }
}
