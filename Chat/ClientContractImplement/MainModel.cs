using ContractClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

            // _friendshipNotAllowed = new ObservableCollection<User>(relationsCustomer.GetContactsByRelationStatus().Response);
        }
        public void Connect()
        {
            var connect = relationsCustomer.Connect();
            if (!connect.IsOk)
            {
                Error?.Invoke("Auth", "error auth");
                return;
            }
            Author = connect.Response;
            Contacts = new ObservableCollection<User>(relationsCustomer.GetUsersByRelationStatus(RelationStatus.Friendship).Response);
            var receive = relationsCustomer.GetUsersByRelationStatus(RelationStatus.FrienshipRequestRecive);
            if (receive.IsOk)
            {
                _friendshipRequestReceive = new ObservableCollection<User>(receive.Response);
            }
            var sent = relationsCustomer.GetUsersByRelationStatus(RelationStatus.FriendshipRequestSent);
            if (sent.IsOk)
            {
                _friendshipRequestSend = new ObservableCollection<User>(sent.Response);
            }

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

        #region Relations






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

        public List<User> FindUsers(String searchQuery)
        {
            var res = relationsCustomer.FindUsers(searchQuery);
            return res.Response;
        }
        // private ObservableCollection<User> _friendshipNotAllowed;

        //  public Collection<User> FriendshipNotAllowed { get => _friendshipNotAllowed; }

        public Collection<User> Friends { get => _contacts; }


        private ObservableCollection<User> _friendshipRequestSend;

        public Collection<User> FriendshipRequestSend => _friendshipRequestSend;

        private ObservableCollection<User> _friendshipRequestReceive;

        public Collection<User> FriendshipRequestReceive => _friendshipRequestReceive;

        public async void AddFriend(String login)
        {
            await Task.Run(() => SendFriendshipRequest(login));
        }
        public async void RemoveFromFriends(String login)
        {
            await Task.Run(() => RemoveFriendship(login));
        }
        public async void BlockUser(String login)
        {
            await Task.Run(() => AddToBlackList(login));
        }
        public async void UnblockUser(String login)
        {
            await Task.Run(() => RemoveRelationType(login));
        }
        public async void RemoveFriendshipRequest(String login)
        {
            await Task.Run(() => RemoveRelationType(login));
        }
        public void ChangeNetworkStatus(NetworkStatus status)
        {
            var res = relationsCustomer.ChangeNetworkStatus(status);
        }

        NetworkStatus _status;
        public NetworkStatus NetworkStatus
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void ChangeRelationStatus(String login, RelationStatus status)
        {
            switch (status)
            {
                case RelationStatus.FriendshipRequestSent:
                    SendFriendshipRequest(login);
                    break;
                case RelationStatus.Friendship:
                    ConfirmFriendship(login);
                    break;
                case RelationStatus.FrienshipRequestRecive:
                    RemoveFriendship(login);
                    break;
                case RelationStatus.BlockedByMe:
                    break;
            }
        }
        private void SendFriendshipRequest(String login)
        {
            var res = relationsCustomer.FriendshipRequest("Hello", login);
            if (res.IsOk)
            {
                FriendshipRequestSend.Add(res.Response);
            }
            else
            {
                Error?.Invoke("Confirm friendship", res.ErrorMessage);
            }
        }
        private void ConfirmFriendship(String login)
        {
            var res = relationsCustomer.ChangeRelationType(login, RelationStatus.Friendship);
            if (res.IsOk)
            {
                var user = FriendshipRequestReceive.FirstOrDefault(x => x.Login == login);
                Contacts.Add(user);
                FriendshipRequestReceive.Remove(user);
            }
            else
            {
                Error?.Invoke("Confirm friendship", res.ErrorMessage);
            }
        }
        private void RemoveFriendship(String login)
        {
            var res = relationsCustomer.ChangeRelationType(login, RelationStatus.FrienshipRequestRecive);
            if (res.IsOk)
            {
                var user = Contacts.FirstOrDefault(x => x.Login == login);
                Application.Current.Dispatcher.Invoke(() =>
                {
                   Contacts.Remove(user);
                FriendshipRequestReceive.Remove(user);                   
                });
               
            }
            else
            {
                Error.Invoke("Remove Friendship error", res.ErrorMessage);
            }
        }
        private void AddToBlackList(String login)
        {
            var res = relationsCustomer.ChangeRelationType(login, RelationStatus.BlockedByMe);
            if (res.IsOk)
            {
                var friend = Contacts.FirstOrDefault(x => x.Login == login);
                if (friend != null)
                {
                    Contacts.Remove(friend);
                }
                var requestReceive = FriendshipRequestReceive.FirstOrDefault(x => x.Login == login);
                if (requestReceive != null)
                {
                    FriendshipRequestReceive.Remove(requestReceive);
                }
                var requestSent = FriendshipRequestSend.FirstOrDefault(x => x.Login == login);
                if (requestSent != null)
                {
                    FriendshipRequestSend.Remove(requestSent);
                }
            }
            else
            {
                Error?.Invoke("BlockUserError error", res.ErrorMessage);
            }
        }
        private void RemoveRelationType(String login)
        {
            var res = relationsCustomer.ChangeRelationType(login, RelationStatus.None);
            if (!res.IsOk)
            {
                Error?.Invoke("BlockUserError error", res.ErrorMessage);
            }
        }
        #endregion


















        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
