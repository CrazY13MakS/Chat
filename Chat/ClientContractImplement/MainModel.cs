using ContractClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientContractImplement
{
    public class ModelMain : INotifyPropertyChanged, IRelationsCallbackModel
    {
        ChatCustomerCallbackService callbackService;
        ChatCustomerService chat;

        AccountRelationsCustomer relationsCustomer;
        AccountRelationsCallback relationsCallback;

        public ModelMain(String token)
        {
            callbackService = new ChatCustomerCallbackService();
            chat = new ChatCustomerService(token, callbackService);
            //  Author = chat.Authentication();

            relationsCallback = new AccountRelationsCallback(this);
            relationsCustomer = new AccountRelationsCustomer(token, relationsCallback);

            Contacts = new ObservableCollection<User>(relationsCustomer.GetFriends().Response);
            _friendshipNotAllowed = new ObservableCollection<User>(relationsCustomer.GetNotAlowedFriends().Response);
        }

        private async void UpdateContacts()
        {
            // Contacts = relationsCustomer
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
                if (_contacts == null)
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
        private ObservableCollection<User> _friendshipNotAllowed;

        public Collection<User> Friends { get => _contacts; }

        public Collection<User> FriendshipNotAllowed { get => _friendshipNotAllowed; }

        public void SendMessage(String body, long conversationId)
        {
            var conv = Conversations.FirstOrDefault(x => x.Id == conversationId);
            if (conv == null)
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


        public void ChangeRelationStatus(String login, RelationStatus status)
        {
            switch (status)
            {
                case RelationStatus.None:

                    break;
                case RelationStatus.Friendship:
                    ConfirmFriendship(login);
                    break;
                case RelationStatus.FriendshipRequestSent:
                    break;
                case RelationStatus.FrienshipRequestRecive:
                    break;
                case RelationStatus.BlockedByMe:
                    break;
                case RelationStatus.BlockedByPartner:
                    break;
                case RelationStatus.BlockedBoth:
                    break;
                default:
                    break;
            }
        }
        private void ConfirmFriendship(String login)
        {
            var res = relationsCustomer.ChangeRelationType(login, RelationStatus.Friendship);
            if (res.IsOk)
            {
                var user = FriendshipNotAllowed.FirstOrDefault(x => x.Login == login);
                Contacts.Add(user);
                FriendshipNotAllowed.Remove(user);
            }
            else
            {
                Error.Invoke("Confirm friendship", res.ErrorMessage);
            }
        }
        private void RemoveFriendship(String login)
        {
            var res = relationsCustomer.ChangeRelationType(login, RelationStatus.FrienshipRequestRecive);
            if (res.IsOk)
            {
                var user = FriendshipNotAllowed.FirstOrDefault(x => x.Login == login);
                Contacts.Add(user);
                FriendshipNotAllowed.Remove(user);
            }
            else
            {
                Error.Invoke("Confirm friendship", res.ErrorMessage);
            }
        }



















        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
