using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ContractClient;
using ContractClient.Contracts;

namespace ClientContractImplement
{
    public class ChatCustomerService //: INotifyPropertyChanged
    {
        public delegate void ErrorHandler(String action, string message);
        public event ErrorHandler Error;

        //void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        //public event PropertyChangedEventHandler PropertyChanged;

        //UserExt _user;
        //public UserExt User
        //{
        //    get
        //    {
        //        return _user;
        //    }
        //    set
        //    {
        //        if (_user != value)
        //        {
        //            _user = value;
        //            RaisePropertyChanged();
        //        }
        //    }
        //}

        IChatService chatService;
        DuplexChannelFactory<IChatService> duplexChannelFactory;
        private readonly String _token;
        public ChatCustomerService(String token, IChatClient client)
        {
            _token = token;
            InstanceContext context = new InstanceContext(client);
            duplexChannelFactory = new DuplexChannelFactory<IChatService>(context, "ClientMessageServiceEndPoint");
            chatService = duplexChannelFactory.CreateChannel();
           // User = chatService.Authentication(token);
        }
        public UserExt Authentication()
        {
            return chatService.Authentication(_token);            
        }

        public async Task<bool> Disconnect()
        {
            bool res = await Task.Run(() => chatService.Disconnect());
            return res;
        }

        public async void SendMessage(String body, long conversationId)
        {
            try
            {
                await Task.Run(()=>chatService.SendMessage(body, conversationId));
            }
            catch (FaultException ex)
            {
                Error.Invoke("SendMessage", "Ошибка сервиса");
            }
            catch (CommunicationException ex)
            {
                Error.Invoke("SendMessage","Ошибка сети");
            }
        }
    }

}
