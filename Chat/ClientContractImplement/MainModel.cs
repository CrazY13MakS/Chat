﻿using ContractClient;
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
    public class ModelMain : INotifyPropertyChanged, IRelationsCallbackModel, IChatCallbackModel
    {
        ChatCustomerCallbackService callbackService;
        ChatCustomerService chat;

        AccountRelationsCustomer relationsCustomer;
        AccountRelationsCallback relationsCallback;

        public ModelMain(String token)
        {
            callbackService = new ChatCustomerCallbackService(this);
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
                Error?.Invoke("Auth", connect.ErrorMessage);
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

            chat.Connect();
            var conv = chat.GetConversations();
            if (conv.IsOk)
            {
                Conversations = new ObservableCollection<Conversation>(conv.Response);
                foreach (var item in Conversations)
                {
                    item.NewMessagesCount = item.Messages.Count(x => x.Status == ConversationReplyStatus.Received);
                }
            }

        }
        private async void UpdateContacts()
        {
            // Contacts = relationsCustomer
        }
        public delegate void ErrorHandler(String action, string message);
        public event ErrorHandler Error;

        #region Chat





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

        public void CreateConversation(String Name, bool IsOpen = false)
        {
            var res = chat.CreateConversation(Name, IsOpen);
            if (res.IsOk)
            {
                Conversations.Add(res.Response);
            }
            else
            {
                Error?.Invoke("Create conversation", res.ErrorMessage);
            }
        }


        public async void InviteFriendToConversation(String Login, long conversationId)
        {
            var res = await Task.Run(() => chat.InviteFriendToConversation(Login, conversationId));
            if (res.IsOk)
            {
                Conversations.FirstOrDefault(x => x.Id == conversationId)?.ParticipantsLogin.Add(Login);
            }
            else
            {
                Error?.Invoke("Invite Friend To Conversation", res.ErrorMessage);
            }
        }

        public async void LeaveConversation(long conversationId)
        {
            var res = await Task.Run(() => chat.LeaveConversation(conversationId));
            if (res.IsOk)
            {
                var conv = Conversations.FirstOrDefault(x => x.Id == conversationId);
                if (conv != null)
                {
                    conv.MyStatus = ConversationMemberStatus.LeftConversation;
                }
            }
            else
            {
                Error?.Invoke("Invite Friend To Conversation", res.ErrorMessage);
            }
        }
        public async void ReadMessage(long conversationId, long messageId)
        {

            var res = await Task.Run(() => Conversations.FirstOrDefault(x => x.Id == conversationId));
            if (res != null)
            {
                var mes = res.Messages.FirstOrDefault(x => x.Id == messageId);
                if (mes != null && mes.Status == ConversationReplyStatus.Received)
                {
                    var response = chat.ReadMessage(messageId);
                    if (response.IsOk)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            mes.Status = ConversationReplyStatus.AlreadyRead;
                            res.NewMessagesCount = res.Messages.Count(x => x.Status == ConversationReplyStatus.Received);
                        });
                    }
                    else
                    {
                        Error?.Invoke("ReadMessage", response.ErrorMessage);
                    }
                }
            }
        }


        public async void SendMessage(String body, long conversationId)
        {
            var conv = Conversations.FirstOrDefault(x => x.Id == conversationId);
            if (conv == null)
            {
                Error.Invoke("Send message", "ConversationId not found error");
                return;
            }
            ConversationReply reply = new ConversationReply()
            {
                Author = Author.Login,
                Body = body,
                ConversationId = conversationId,
                SendingTime = DateTimeOffset.Now,
                Status = ConversationReplyStatus.Sending
            };
            conv.Messages.Add(reply);
            var res = await Task.Run(() => chat.SendMessage(body, conversationId));
            if (res.IsOk)
            {
                reply.Status = ConversationReplyStatus.Sent;
            }
            else
            {
                reply.Status = ConversationReplyStatus.SendingError;
            }

        }

        #endregion

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
            // await Task.Run(() => SendFriendshipRequest(login));

            var res = await Task.Run(() => relationsCustomer.FriendshipRequest("Hello", login));
            if (res.IsOk)
            {
                Application.Current.Dispatcher.Invoke(() => FriendshipRequestSend.Add(res.Response));
            }
            else
            {
                Error?.Invoke("Confirm friendship", res.ErrorMessage);
            }

        }
        public async void RemoveFromFriends(String login)
        {
            // await Task.Run(() => RemoveFriendship(login));
            var res = await Task.Run(() => relationsCustomer.ChangeRelationType(login, RelationStatus.FrienshipRequestRecive));
            if (res.IsOk)
            {
                var user = Contacts.FirstOrDefault(x => x.Login == login);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    user.RelationStatus = RelationStatus.FrienshipRequestRecive;
                    Contacts.Remove(user);
                    FriendshipRequestReceive.Add(user);
                });

            }
            else
            {
                Error.Invoke("Remove Friendship error", res.ErrorMessage);
            }
        }
        public async void BlockUser(String login)
        {
            // await Task.Run(() => AddToBlackList(login)); 
            var res = await Task.Run(() => relationsCustomer.ChangeRelationType(login, RelationStatus.BlockedByMe));
            if (res.IsOk)
            {
                var friend = Contacts.FirstOrDefault(x => x.Login == login);
                var requestReceive = FriendshipRequestReceive.FirstOrDefault(x => x.Login == login);
                var requestSent = FriendshipRequestSend.FirstOrDefault(x => x.Login == login);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (friend != null)
                    {
                        Contacts.Remove(friend);
                    }
                    if (requestReceive != null)
                    {
                        FriendshipRequestReceive.Remove(requestReceive);
                    }
                    if (requestSent != null)
                    {
                        FriendshipRequestSend.Remove(requestSent);
                    }
                });
            }
            else
            {
                Error?.Invoke("BlockUserError error", res.ErrorMessage);
            }
        }
        public async void UnblockUser(String login)
        {
            // await Task.Run(() => RemoveRelationType(login));
            var res = await Task.Run(() => relationsCustomer.ChangeRelationType(login, RelationStatus.None));
            if (!res.IsOk)
            {
                Error?.Invoke("BlockUserError error", res.ErrorMessage);
            }
        }
        public async void RemoveFriendshipRequest(String login)
        {
            //  await Task.Run(() => RemoveRelationType(login));
            var res = await Task.Run(() => relationsCustomer.ChangeRelationType(login, RelationStatus.None));
            if (!res.IsOk)
            {

                Error?.Invoke("BlockUserError error", res.ErrorMessage);
            }
            else
            {
                var requestReceive = FriendshipRequestReceive.FirstOrDefault(x => x.Login == login);
                var requestSent = FriendshipRequestSend.FirstOrDefault(x => x.Login == login);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (requestReceive != null)
                    {
                        FriendshipRequestReceive.Remove(requestReceive);
                    }
                    if (requestSent != null)
                    {
                        FriendshipRequestSend.Remove(requestSent);
                    }
                });
            }
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

        Collection<Conversation> IChatCallbackModel.Conversations => _conversations;

        public async void ChangeRelationStatus(String login, RelationStatus status)
        {
            await Task.Run(() =>
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
            });
        }
        private void SendFriendshipRequest(String login)
        {
            var res = relationsCustomer.FriendshipRequest("Hello", login);
            if (res.IsOk)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    FriendshipRequestSend.Add(res.Response);
                });
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
                Application.Current.Dispatcher.Invoke(() =>
                {
                    user.RelationStatus = RelationStatus.Friendship;
                    Contacts.Add(user);
                    FriendshipRequestReceive.Remove(user);
                });
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
                Application.Current.Dispatcher.Invoke(() =>
                {
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
                });
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
            else
            {
                var friend = Contacts.FirstOrDefault(x => x.Login == login);
                var receive = FriendshipRequestReceive.FirstOrDefault(x => x.Login == login);
                var sent = FriendshipRequestSend.FirstOrDefault(x => x.Login == login);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (friend != null)
                    {
                        Friends.Remove(friend);
                    }
                    if (receive != null)
                    {
                        FriendshipRequestReceive.Remove(receive);
                    }
                    if (sent != null)
                    {
                        FriendshipRequestSend.Remove(sent);
                    }
                });
            }
        }

        public async void UpdateProfile(UserExt user)
        {
            var res = await Task.Run(() => relationsCustomer.UpdateProfile(user));
            if (res.IsOk)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Author.BirthDate = user.BirthDate;
                    Author.Icon = user.Icon;
                    Author.Phone = user.Phone;
                    Author.Name = user.Name;
                });
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
