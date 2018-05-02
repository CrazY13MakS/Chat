﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientContractImplement;
using ContractClient;

namespace ChatClient.ViewModel
{
    class ChatMainWindowViewModel:ViewModelBase
    {
        ClientModel.ModelMain model = new ClientModel.ModelMain(App.Token);

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
    }
}
