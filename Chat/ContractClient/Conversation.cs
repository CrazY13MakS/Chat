using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace ContractClient
{
    public class Conversation : INotifyPropertyChanged
    {

        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public Conversation()
        {
            Messages = new ObservableCollection<ConversationReply>();

        }

        //private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (Messages != null)
        //    {
        //        NewMessagesCount = Messages.Count(x => x.Status == ConversationReplyStatus.Received);
        //    }
        //}
        int _newMessagesCount;
        public int NewMessagesCount
        {
            get
            {
                return _newMessagesCount;
            }
            set
            {
                if (_newMessagesCount != value)
                {
                    _newMessagesCount = value;
                    RaisePropertyChanged();
                }
            }
        }

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
        ObservableCollection<ConversationReply> _messages;
        public ObservableCollection<ConversationReply> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                if (_messages != value)
                {
                    //if (_messages != null)
                    //{
                    //    _messages.CollectionChanged -= Messages_CollectionChanged;
                    //}
                    _messages = value;
                    RaisePropertyChanged();
                    //if (_messages != null)
                    //{
                    //    _messages.CollectionChanged += Messages_CollectionChanged;
                    //}
                    if (_messages != null)
                    {
                        NewMessagesCount = Messages.Count(x => x.Status == ConversationReplyStatus.Received);
                    }
                }
            }
        }

        public DateTimeOffset LastChange { get; set; }
    }
}
