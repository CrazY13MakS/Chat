using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace ContractClient
{
    [DataContract]
    public class User : INotifyPropertyChanged
    {

        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public String Login { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public byte[] Icon { get; set; }

        [DataMember]
        public long? ConversationId { get; set; }

        [DataMember]
        public RelationStatus RelationStatus { get; set; }

        [DataMember]
        NetworkStatus _networkStatus;
        public NetworkStatus NetworkStatus
        {
            get
            {
                return _networkStatus;
            }
            set
            {
                if (_networkStatus != value)
                {
                    _networkStatus = value;
                    RaisePropertyChanged();
                }
            }
        }


        // [DataMember]
        // public DateTime LastDialogChange { get; set; }
    }
}
