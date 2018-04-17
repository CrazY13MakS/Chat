using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public class Conversation
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Descriptiom { get; set; }
        public byte[] Icon { get; set; }
        public User Partner { get; set; }
        public bool IsConversation { get; set; }
        public List<String> ParticipantsLogin { get; set; }
        public ConversationMemberStatus MyStatus { get; set; }
        public DateTime LastChange { get; set; }
    }
}
