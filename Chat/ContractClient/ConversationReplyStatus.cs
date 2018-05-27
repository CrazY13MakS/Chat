using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public enum ConversationReplyStatus
    {
        Sent=0 ,
        Delivered=1 ,
        AlreadyRead=2 ,
        Sending=3 ,
        Received=4,
        SendingError=5,
        SystemMessage=6

    }
}
