using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace ContractClient
{
    [DataContract]
    public class User
    {
        [DataMember]
        public String Login { get; set; }

        [DataMember]
        public String Name { get; set; }       

        [DataMember]
        public byte[] Icon { get; set; }

        [DataMember]
        public long ConversationId { get; set; }

        [DataMember]
        public RelationStatus RelationStatus { get; set; }

        [DataMember]
        public NetworkStatus NetworkStatus { get; set; }

        [DataMember]
        public DateTime LastDialogChange { get; set; }
    }
}
