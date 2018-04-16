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
        public List<String> ParticipantsLogin { get; set; }
    }
}
