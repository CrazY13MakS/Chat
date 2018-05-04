using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ContractClient;
using ContractClient.Contracts;
namespace ClientContractImplement
{
    public class AccountRelationsCustomer : IDisposable
    {
        //Uri address = new Uri("http://localhost:4000/Auth");
        //NetTcpBinding binding = new NetTcpBinding();
        DuplexChannelFactory<IRelations> factory = null;
        IRelations channel;
        InstanceContext context;
        private readonly String connectionString = "ClientAccUpdateEndPoint";
        private readonly String token;
        public AccountRelationsCustomer(String token, IRelationsCallback callback)
        {
            this.token = token;
            context = new InstanceContext(callback);
            factory = new DuplexChannelFactory<IRelations>(context, connectionString);
            factory.Faulted += Factory_Faulted;
            channel = factory.CreateChannel();
            channel.Authentication(token);
        }
        //public AccountRelationsCustomer(String token)
        //{
        //    factory = new DuplexChannelFactory<IRelations>("ClientAccUpdateEndPoint");
        //    factory.Faulted += Factory_Faulted;
        //    channel = factory.CreateChannel();
        //    channel.Authentication(token);
        //}

        private void Factory_Faulted(object sender, EventArgs e)
        {
            try
            {
                factory = new DuplexChannelFactory<IRelations>(context, connectionString);
                channel = factory.CreateChannel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(this, ex.Message);
            }
        }

        public void Dispose()
        {
            factory?.Close();
        }

        public OperationResult<User> FriendshipRequest(String body, String userLogin)
        {
            OperationResult<User> result = null;
            try
            {
                result = channel.FriendshipRequest(body, userLogin);
            }
            catch (CommunicationException ex)
            {
                ReloadChannel();
                var res = FriendshipRequest(body, userLogin);
                if (!res.IsOk)
                {
                    res = new OperationResult<User>(null, false, ex.Message);
                }
                return res;
            }
            return result;
        }
        private void ReloadChannel()
        {
            //channel = factory.CreateChannel();
            Factory_Faulted(this, EventArgs.Empty);
            channel.Authentication(token);
        }

        public OperationResult<bool> ChangeNetworkStatus(NetworkStatus status)
        {
            try
            {
                return channel.ChangeNetworkStatus(status);
            }
            catch (CommunicationException ex)
            {
                ReloadChannel();
                var res = ChangeNetworkStatus(status);
                if (!res.IsOk)
                {
                    res = new OperationResult<bool>(false, false, ex.Message);
                }
                return res;
            }
        }


        public OperationResult<List<User>> FindUsers(String param)
        {
            try
            {
                return channel.FindUsers(param);
            }
            catch (CommunicationException ex)
            {
                ReloadChannel();
                var res = FindUsers(param);
                if (!res.IsOk)
                {
                    res = new OperationResult<List<User>>(new List<User>(), false, ex.Message);
                }
                return res;
            }
        }

        public OperationResult<List<User>> GetFriends()
        {
            try
            {
                return channel.GetFriends();
            }
            catch (CommunicationException ex)
            {
                ReloadChannel();
                var res = channel.GetFriends();
                if (!res.IsOk)
                {
                    res = new OperationResult<List<User>>(new List<User>(), false, ex.Message);
                }
                return res;
            }
        }
        public OperationResult<List<User>> GetNotAlowedFriends()
        {
            try
            {
                return channel.GetNotAllowedFriends();
            }
            catch (CommunicationException ex)
            {
                ReloadChannel();
                var res = channel.GetNotAllowedFriends();
                if (!res.IsOk)
                {
                    res = new OperationResult<List<User>>(new List<User>(), false, ex.Message);
                }
                return res;
            }
        }
        public OperationResult<bool> UpdateProfile(UserExt user)
        {
            try
            {
                return channel.UpdateProfile(user);
            }
            catch (CommunicationException ex)
            {
                ReloadChannel();
                var res = UpdateProfile(user);
                if (!res.IsOk)
                {
                    res = new OperationResult<bool>(false, false, ex.Message);
                }
                return res;
            }
        }

        public OperationResult<bool> ChangeRelationType(String login, RelationStatus status)
        {
            try
            {
                return channel.ChangeRelationType(login, status);
            }
            catch (CommunicationException ex)
            {
                ReloadChannel();
                return new OperationResult<bool>(false, false, "Connection error");
            }
        }


    }
}
