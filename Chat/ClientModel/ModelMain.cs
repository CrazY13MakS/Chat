using ContractClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClientContractImplement;
namespace ClientModel
{
    public class ModelMain:INotifyPropertyChanged
    {
        ChatCustomerCallbackService callbackService;
        ChatCustomerService chat;
        public ModelMain(String token)
        {
            callbackService = new ChatCustomerCallbackService();
            chat = new ChatCustomerService(token, callbackService);
            Author = chat.Authentication();
        }

        public delegate void ErrorHandler(String action, string message);
        public event ErrorHandler Error;

        ObservableCollection<Conversation> _conversations;
        public ObservableCollection<Conversation> Conversations
        {
            get
            {
                return _conversations;
            }
            set
            {
                if (_conversations != value)
                {
                    _conversations = value;
                    RaisePropertyChanged();
                }
            }
        }

        ObservableCollection<User> _contacts;
        public ObservableCollection<User> Contacts
        {
            get
            {
                if(_contacts==null)
                {
                   
                }
                return _contacts;
            }
            set
            {
                if (_contacts != value)
                {
                    _contacts = value;
                    RaisePropertyChanged();
                }
            }
        }
        UserExt _author;
        public UserExt Author
        {
            get
            {
                return _author;
            }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void SendMessage(String body, long conversationId)
        {
         var conv=   Conversations.FirstOrDefault(x => x.Id == conversationId);
            if(conv==null)
            {
                Error.Invoke("Send message", "ConversationId not found error");
            }
            ConversationReply reply = new ConversationReply()
            {
                Author = Author.Login,
                Body = body,
                ConversationId = conversationId,
                SendingTime = DateTime.UtcNow,
                Status = ConversationReplyStatus.Sendidg
            };
            conv.Messages.Add(reply);


        }




















        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
