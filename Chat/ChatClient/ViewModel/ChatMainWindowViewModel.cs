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
        public ObservableCollection<Conversation> Conversations
        {
            get
            {
                return model.Conversations;
            }
        }

        public ObservableCollection<User> Contacts
        {
            get
            {

                return model.Contacts;
            }
        }
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
