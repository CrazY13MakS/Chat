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
        public AccountRelationsCustomer(String token, IRelationsCallback callback)
        {
            context = new InstanceContext(callback);
            factory = new DuplexChannelFactory<IRelations>(context,connectionString);
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
            channel = factory.CreateChannel();
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


        //OperationResult<bool> FrienshipResponse(String login, bool isConfirmed)
        //{
        //    try
        //    {
        //        return channel.FrienshipResponse(login, isConfirmed);
        //    }
        //    catch (CommunicationException ex)
        //    {
        //        ReloadChannel();
        //        var res = FrienshipResponse(login, isConfirmed);
        //        if (!res.IsOk)
        //        {
        //            res = new OperationResult<bool>(false, false, ex.Message);
        //        }
        //        return res;
        //    }
        //}


        //OperationResult<bool> BlockUser(String login)
        //{
        //    try
        //    {
        //        return channel.BlockUser(login);
        //    }
        //    catch (CommunicationException ex)
        //    {
        //        ReloadChannel();
        //        var res = channel.BlockUser(login);
        //        if (!res.IsOk)
        //        {
        //            res = new OperationResult<bool>(false, false, ex.Message);
        //        }
        //        return res;
        //    }
        //}


        //OperationResult<bool> UnBlockUser(String login)
        //{
        //    try
        //    {
        //        return channel.UnBlockUser(login);
        //    }
        //    catch (CommunicationException ex)
        //    {
        //        ReloadChannel();

        //        var res = UnBlockUser(login);
        //        if (!res.IsOk)
        //        {
        //            res = new OperationResult<bool>(false, false, ex.Message);
        //        }
        //        return res;
        //    }
        //}

    }
}
