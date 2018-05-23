using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace ContractClient
{
    public class Conversation
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public String Descriptiom { get; set; }
        public byte[] Icon { get; set; }
        public String IconPathLocal { get; set; }
        public User Partner { get; set; }
        public String PartnerLogin { get; set; }
                                           //  public bool IsOpenConversation { get; set; }
        public ConversationType ConversationType { get; set; }
        public List<String> ParticipantsLogin { get; set; }
        public ConversationMemberStatus MyStatus { get; set; }
        public ObservableCollection<ConversationReply> Messages { get; set; }
        public DateTimeOffset LastChange { get; set; }
    }
}
