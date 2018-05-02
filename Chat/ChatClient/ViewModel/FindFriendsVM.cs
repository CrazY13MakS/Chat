using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ChatClient.Infrastructure;
using ContractClient;

namespace ChatClient.ViewModel
{
    class FindFriendsVM : ViewModelBase
    {
        ClientContractImplement.AccountUpdateCustomer account = new ClientContractImplement.AccountUpdateCustomer(App.Token);
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
            var res = account.FindUsers(SearchQuery);
            Users = new ObservableCollection<User>(res.Response);

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
            var res = account.FriendshipRequest("hello", SelectedUser.Login);

        }

        private bool CanExecuteSendFriendRequestCommand(object parametr)
        {
            return SelectedUser != null && SelectedUser.RelationStatus != RelationStatus.None;
        }
    }
}
