using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        IChatService chatService;
        DuplexChannelFactory<IChatService> duplexChannelFactory;
        private readonly String connectionString = "ClientMessageServiceEndPoint";
        private readonly String _token;
        IChatCallback _client;
        public ChatCustomerService(String token, IChatCallback client)
        {
            _token = token;
            _client = client;

        }
        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        UserExt _user;
        public UserExt User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    RaisePropertyChanged();
                }
            }
        }



        public void Connect()
        {
            InstanceContext context = new InstanceContext(_client);
            duplexChannelFactory = new DuplexChannelFactory<IChatService>(context, connectionString);
            chatService = duplexChannelFactory.CreateChannel();
            OperationResult<UserExt> res;
            try
            {
                res = chatService.Authentication(_token);
            }
            catch (Exception ex)
            {
                res = new OperationResult<UserExt>(null, false, ex.Message);
            }
            if (res.IsOk)
            {
                User = res.Response;
            }
            else
            {
                Error?.Invoke("Chat Authentication", res.ErrorMessage);
            }
        }
        public OperationResult<bool> SendMessage(String body, long conversationId)
        {
            String ErrorMessage = "UnknownError";
            try
            {
                var res = chatService.SendMessage(body, conversationId);
                return res;
            }
            catch (FaultException ex)
            {
                ErrorMessage = "Ошибка сервиса";
                Error.Invoke("SendMessage", "Ошибка сервиса");
            }
            catch (CommunicationException ex)
            {
                ErrorMessage = "Ошибка сети";
                Error.Invoke("SendMessage", "Ошибка сети");
            }
            return new OperationResult<bool>(false, false, ErrorMessage);
        }




        public OperationResult<List<Conversation>> GetConversations()
        {
            try
            {
                return chatService.GetConversations();
            }
            catch (CommunicationException ex)
            {
                return new OperationResult<List<Conversation>>(null, false, ex.Message);
            }
        }



        public OperationResult<List<ConversationReply>> GetMessages(long conversationId)
        {
            try
            {
                return chatService.GetMessages(conversationId);
            }
            catch (CommunicationException ex)
            {
                return new OperationResult<List<ConversationReply>>(null, false, ex.Message);
            }
        }


        public OperationResult<Conversation> CreateDialog(String Login)
        {
            throw new NotImplementedException();
        }


        public OperationResult<Conversation> CreateConversation(String Name, bool IsOpen = false)
        {
            try
            {
                return chatService.CreateConversation(Name, IsOpen);
            }
            catch (CommunicationException ex)
            {
                return new OperationResult<Conversation>(null, false, ex.Message);
            }
        }

        public OperationResult<bool> InviteFriendToConversation(String Login, long conversationId)
        {
            try
            {
                return chatService.InviteFriendToConversation(Login, conversationId);
            }
            catch (CommunicationException ex)
            {
                return new OperationResult<bool>(false, false, ex.Message);
            }
        }

        public OperationResult<bool> LeaveConversation(long conversationId)
        {
            try
            {
                return chatService.LeaveConversation(conversationId);
            }
            catch (CommunicationException ex)
            {
                return new OperationResult<bool>(false, false, ex.Message);
            }
        }
        public OperationResult<bool> ReadMessage(long messageId)
        {
            try
            {
                return chatService.ReadMessage(messageId);
            }
            catch (CommunicationException ex)
            {
                return new OperationResult<bool>(false, false, ex.Message);
            }
        }

    }

}
