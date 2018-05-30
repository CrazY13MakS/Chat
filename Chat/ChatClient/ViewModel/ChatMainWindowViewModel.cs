using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatClient.Infrastructure;
using ClientContractImplement;
using ContractClient;

namespace ChatClient.ViewModel
{
    class ChatMainWindowViewModel : ViewModelBase
    {
        ModelMain model;// = new ModelMain(App.Token);
        public ChatMainWindowViewModel()
        {
            model = new ModelMain(App.Token);
            model.Connect();
        }
        //  public ModelMain ModelMain { get { return model; } }

        RelayCommand _editUser;
        public ICommand EditUserCommand
        {
            get
            {
                if (_editUser == null)
                {
                    _editUser = new RelayCommand(ExecuteEditUserCommand);
                }
                return _editUser;
            }
        }

        private  void ExecuteEditUserCommand(object parametr)
        {
            // var window=await Task.Run(()=> App.DisplayWindowHelper.CreateWindowInstanceWithVM(new UserEditViewModel() { User = Author }));
            var window = new  View.UserEdit();
           //var res= App.DisplayWindowHelper.ShowModalPresentation(new UserEditViewModel() { User = Author });
           //if( res.Result==true)
           // {
               
           // }
            window.DataContext=new UserEditViewModel() { User = Author };
            if(window.ShowDialog()==true)
            {
                dynamic dyn = window.DataContext;
                model.UpdateProfile(dyn.User as UserExt);
            }
        }

        #region Conversations
        public ObservableCollection<Conversation> Conversations
        {
            get
            {
                return model.Conversations;
            }
        }

        private Conversation _selectedConversation;

        public Conversation SelectedConversation
        {
            get { return _selectedConversation; }
            set
            {
                if (_selectedConversation == value)
                {
                    return;
                }
                _selectedConversation = value;
                OnPropertyChanged();
                UpdateParticipantsList();
            }
        }

        private String _message;

        public String Message
        {
            get { return _message; }
            set
            {
                if (_message == value)
                {
                    return;
                }
                _message = value;
                OnPropertyChanged();
            }
        }










        RelayCommand _sendMessage;
        public ICommand SendMessageCommand
        {
            get
            {
                if (_sendMessage == null)
                {
                    _sendMessage = new RelayCommand(ExecuteSendMessageCommand, CanExecuteSendMessageCommand);
                }
                return _sendMessage;
            }
        }

        private void ExecuteSendMessageCommand(object parametr)
        {
            var conversation = parametr as Conversation;
            model.SendMessage(Message, conversation.Id);
            Message = String.Empty;
        }

        private bool CanExecuteSendMessageCommand(object parametr)
        {
            return !String.IsNullOrWhiteSpace(Message) && parametr is Conversation conversation && (conversation.MyStatus == ConversationMemberStatus.Active || conversation.MyStatus == ConversationMemberStatus.Admin);
        }


        RelayCommand _readMessage;
        public ICommand ReadMessageCommand
        {
            get
            {
                if (_readMessage == null)
                {
                    _readMessage = new RelayCommand(ExecuteReadMessageCommand);
                }
                return _readMessage;
            }
        }

        private void ExecuteReadMessageCommand(object parametr)
        {
            if (parametr is ConversationReply reply && reply.Status == ConversationReplyStatus.Received)
            {
                model.ReadMessage(reply.ConversationId, reply.Id);
            }
        }



        #region Add Participants
        List<String> _participantsList;
        public List<String> ParticipantsList
        {
            get
            {
                return _participantsList;
            }
            set
            {
                if (_participantsList != value)
                {
                    _participantsList = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateParticipantsList()
        {
            if (SelectedConversation != null && SelectedConversation.ConversationType != ConversationType.Dialog )
            {
                if(SelectedConversation.ParticipantsLogin==null)
                {
                    SelectedConversation.ParticipantsLogin = new List<string> { Author.Login };
                }
                ParticipantsList = new List<string>(Contacts.Select(x => x.Login).Except(SelectedConversation.ParticipantsLogin));
            }
            else
            {
                ParticipantsList = new List<string>();
            }
        }

        RelayCommand _addParticipant;
        public ICommand AddPartisipantCommand
        {
            get
            {
                if (_addParticipant == null)
                {
                    _addParticipant = new RelayCommand(ExecuteAddPartisipantsCommand, CanExecuteAddPartisipantsCommand);
                }
                return _addParticipant;
            }
        }

        private void ExecuteAddPartisipantsCommand(object parametr)
        {
            bool res = Contacts.Any(x => x.Login == parametr as String);
            bool res2 = !SelectedConversation.ParticipantsLogin.Contains(parametr as String);
            if (res && res2)
            {
                model.InviteFriendToConversation(parametr as String, SelectedConversation.Id);
            }
        }

        private bool CanExecuteAddPartisipantsCommand(object parametr)
        {
            var login = parametr as String;
            if (String.IsNullOrWhiteSpace(login))
            {
                return false;
            }
            bool res = SelectedConversation != null
                && (((SelectedConversation.MyStatus == ConversationMemberStatus.Active || SelectedConversation.MyStatus == ConversationMemberStatus.Admin)
                        && SelectedConversation.ConversationType == ConversationType.OpenConversation)
                        || (SelectedConversation.MyStatus == ConversationMemberStatus.Admin
                                  && SelectedConversation.ConversationType == ConversationType.PrivateConversation)
                        )
                        && !SelectedConversation.ParticipantsLogin.Contains(login);
            return res;
        }

        #endregion
        #region Create New Conversation
        String _newConvName;
        public String NewConversationName
        {
            get
            {
                return _newConvName;
            }
            set
            {
                if (_newConvName != value)
                {
                    _newConvName = value;
                    OnPropertyChanged();
                }
            }
        }

        bool _isOpenNewConversation;
        public bool IsOpenNewConversation
        {
            get
            {
                return _isOpenNewConversation;
            }
            set
            {
                if (_isOpenNewConversation != value)
                {
                    _isOpenNewConversation = value;
                    OnPropertyChanged();
                }
            }
        }


        RelayCommand _createConversation;
        public ICommand CreateConversationCommand
        {
            get
            {
                if (_createConversation == null)
                {
                    _createConversation = new RelayCommand(ExecuteCreateConversationCommand);
                }
                return _createConversation;
            }
        }

        private void ExecuteCreateConversationCommand(object parametr)
        {
            model.CreateConversation(NewConversationName, IsOpenNewConversation);
            IsOpenNewConversation = false;
            NewConversationName = String.Empty;
        }

        private bool CanExecuteCreateConversationCommand(object parametr)
        {
            return !String.IsNullOrWhiteSpace(NewConversationName);
        }
        #endregion
        #endregion

        public UserExt Author
        {
            get
            {
                return model.Author;
            }
        }
        //public ObservableCollection<User> FriendshipNotAllowed
        //{
        //    get
        //    {
        //        return model.FriendshipNotAllowed as ObservableCollection<User>;
        //    }
        //}

        #region Relation Data
        public ObservableCollection<User> Contacts
        {
            get
            {

                return model.Contacts;
            }
        }

        public ObservableCollection<User> FriendshipRequestReceive
        {
            get
            {
                return model.FriendshipRequestReceive as ObservableCollection<User>;
            }
        }

        public ObservableCollection<User> FriendshipRequestSent
        {
            get
            {
                return model.FriendshipRequestSend as ObservableCollection<User>;
            }
        }
        #endregion


    




        #region searchTab

        String _searchQuery;
        public String SearchQuery
        {
            get
            {
                return _searchQuery;
            }
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                }
            }
        }

        ObservableCollection<User> _searchUsers;
        public ObservableCollection<User> SearchUsers
        {
            get
            {
                return _searchUsers;
            }
            set
            {
                if (_searchUsers != value)
                {
                    _searchUsers = value;
                    OnPropertyChanged();
                }
            }
        }

        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser == value)
                {
                    return;
                }
                _selectedUser = value;
                OnPropertyChanged();
            }
        }


        RelayCommand _findCommand;
        public ICommand FindCommand
        {
            get
            {
                if (_findCommand == null)
                {
                    _findCommand = new RelayCommand(ExecuteFindCommand, CanExecuteFindCommand);
                }
                return _findCommand;
            }
        }

        private void ExecuteFindCommand(object parametr)
        {
            var res = model.FindUsers(SearchQuery);
            SearchUsers = new ObservableCollection<User>(res);

        }

        private bool CanExecuteFindCommand(object parametr)
        {
            return !String.IsNullOrWhiteSpace(SearchQuery);
        }


        RelayCommand _sendFriendRequestCommand;
        public ICommand SendFriendRequestCommand
        {
            get
            {
                if (_sendFriendRequestCommand == null)
                {
                    _sendFriendRequestCommand = new RelayCommand(ExecuteSendFriendRequestCommand, CanExecuteSendFriendRequestCommand);
                }
                return _sendFriendRequestCommand;
            }
        }

        private void ExecuteSendFriendRequestCommand(object parametr)
        {
            // var res = service.FriendshipRequest("hello", SelectedUser.Login);
            var user = parametr as User;

            model.ChangeRelationStatus(user.Login, RelationStatus.FriendshipRequestSent);

        }

        private bool CanExecuteSendFriendRequestCommand(object parametr)
        {
            return parametr is User user && user.RelationStatus == RelationStatus.None;
        }
        #endregion


        #region Change relation type

        RelayCommand _confirmFriendship;
        public ICommand ConfirmFriendshipCommand
        {
            get
            {
                if (_confirmFriendship == null)
                {
                    _confirmFriendship = new RelayCommand(ExecuteConfirmFriendshipCommand, CanExecuteConfirmFriendshipCommand);
                }
                return _confirmFriendship;
            }
        }

        private void ExecuteConfirmFriendshipCommand(object parametr)
        {
            var user = parametr as User;
            model.ChangeRelationStatus(user.Login, RelationStatus.Friendship);
        }

        private bool CanExecuteConfirmFriendshipCommand(object parametr)
        {
            return parametr is User user && user.RelationStatus == RelationStatus.FrienshipRequestRecive;
        }

        RelayCommand _removeFromFriends;
        public ICommand RemoveFromFriendsCommand
        {
            get
            {
                if (_removeFromFriends == null)
                {
                    _removeFromFriends = new RelayCommand(ExecuteRemoveFromFriendsCommand, CanExecuteRemoveFromFriendsCommand);
                }
                return _removeFromFriends;
            }
        }

        private void ExecuteRemoveFromFriendsCommand(object parametr)
        {
            var user = parametr as User;
            model.RemoveFromFriends(user.Login);
        }

        private bool CanExecuteRemoveFromFriendsCommand(object parametr)
        {
            return parametr is User user && user.RelationStatus == RelationStatus.Friendship;
        }




        RelayCommand _removeRelationRequest;
        public ICommand RemoveRelationRequestCommand
        {
            get
            {
                if (_removeRelationRequest == null)
                {
                    _removeRelationRequest = new RelayCommand(ExecuteRemoveRelationRequestCommand, CanExecuteRemoveRelationRequestCommand);
                }
                return _removeRelationRequest;
            }
        }

        private void ExecuteRemoveRelationRequestCommand(object parametr)
        {
            var user = parametr as User;
            model.RemoveFriendshipRequest(user.Login);
        }

        private bool CanExecuteRemoveRelationRequestCommand(object parametr)
        {
            return parametr is User user && (user.RelationStatus == RelationStatus.FriendshipRequestSent || user.RelationStatus == RelationStatus.FrienshipRequestRecive);
        }

        RelayCommand _blockUser;
        public ICommand BlockUserCommand
        {
            get
            {
                if (_blockUser == null)
                {
                    _blockUser = new RelayCommand(ExecuteBlockUserCommand, CanExecuteBlockUserCommand);
                }
                return _blockUser;
            }
        }

        private void ExecuteBlockUserCommand(object parametr)
        {
            var user = parametr as User;
            model.BlockUser(user.Login);
        }

        private bool CanExecuteBlockUserCommand(object parametr)
        {
            return parametr is User user && user.RelationStatus != RelationStatus.BlockedByMe;
        }


        RelayCommand _unblockUser;
        public ICommand UnBlockUserCommand
        {
            get
            {
                if (_unblockUser == null)
                {
                    _unblockUser = new RelayCommand(ExecuteUnBlockUserCommand, CanExecuteUnBlockUserCommand);
                }
                return _unblockUser;
            }
        }

        private void ExecuteUnBlockUserCommand(object parametr)
        {
            var user = parametr as User;
            model.UnblockUser(user.Login);
        }

        private bool CanExecuteUnBlockUserCommand(object parametr)
        {
            return parametr is User user && (user.RelationStatus == RelationStatus.BlockedByMe || user.RelationStatus == RelationStatus.BlockedBoth);
        }


        #endregion






        RelayCommand _testCallback;
        public ICommand TestCallback
        {
            get
            {
                if (_testCallback == null)
                {
                    _testCallback = new RelayCommand((x) =>
                    {
                        model.ChangeNetworkStatus(NetworkStatus.Busy);
                    });
                }
                return _testCallback;
            }
        }
    }
}
