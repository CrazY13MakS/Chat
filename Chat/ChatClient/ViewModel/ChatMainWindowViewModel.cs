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
    }
}
