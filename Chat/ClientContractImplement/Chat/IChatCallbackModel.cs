using ContractClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientContractImplement
{
   public interface IChatCallbackModel
    {
        Collection<Conversation> Conversations { get; }

    }
}
