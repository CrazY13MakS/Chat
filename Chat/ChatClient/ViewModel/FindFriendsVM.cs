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
    class FindFriendsVM:ViewModelBase
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
            var res  = account.FindUsers(SearchQuery);
            Users = new ObservableCollection<User>( res.Response);
            
        }

        private bool CanExecuteFindCommand(object parametr)
        {
            return !String.IsNullOrWhiteSpace(SearchQuery);
        }
    }
}
