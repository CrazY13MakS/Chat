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
        ModelMain model = new ModelMain(App.Token);
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
        public ObservableCollection<User> FriendshipNotAllowed
        {
            get
            {
                return model.FriendshipNotAllowed as ObservableCollection<User>;
            }
        }









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
            var user = parametr as User;

            return user != null && user.RelationStatus == RelationStatus.FrienshipRequestRecive;
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

        ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                if (_users != value)
                {
                    _users = value;
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
            Users = new ObservableCollection<User>(res);

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
                    _sendFriendRequestCommand = new RelayCommand(ExecuteFindCommand, CanExecuteFindCommand);
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
            return SelectedUser != null && SelectedUser.RelationStatus != RelationStatus.None;
        }
        #endregion
    }
}
