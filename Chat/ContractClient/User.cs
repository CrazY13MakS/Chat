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
        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }


        [DataMember]
        byte[] _icon;
        public byte[] Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    RaisePropertyChanged();
                }
            }
        }


        [DataMember]
        public long? ConversationId { get; set; }

        [DataMember]
      //  public RelationStatus RelationStatus { get; set; }
        RelationStatus _relationStatus;
        public RelationStatus RelationStatus
        {
            get
            {
                return _relationStatus;
            }
            set
            {
                if (_relationStatus != value)
                {
                    _relationStatus = value;
                    RaisePropertyChanged();
                }
            }
        }

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
