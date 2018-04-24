using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public class ConversationReply
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public String Body { get; set; }
        public String AuthorLogin { get; set; }
        public DateTime SendingTime { get; set; }
        public ConversationReplyStatus Status { get; set; }
    }
}
