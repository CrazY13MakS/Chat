using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public enum ConversationReplyStatus
    {
        Sent = 1,
        Delivered = 2,
        AlreadyRead = 3,
        Sendidg = 4,
        SendingError = 5
    }
}
