using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ContractClient.Contracts;
namespace ClientContractImplement
{
    public class ClientAuthSercive
    {

        //Uri address = new Uri("http://localhost:4000/Auth");
        //NetTcpBinding binding = new NetTcpBinding();
        ChannelFactory<IAuthService> factory = null;
        IAuthService channel;

        public ClientAuthSercive()
        {
            factory = new ChannelFactory<IAuthService>("ClientAuthEndPoint");
            channel = factory.CreateChannel();
        }

        public OperationResult<String> LogIn(string login, string password)
        {
            String result=String.Empty;
            try
            {
                 result = channel.LogIn(login, password);
                if (result.Contains("Error"))
                {
                    return new OperationResult<string>(result, false, result);
                }
                return new OperationResult<string>(result);
            }
            catch (FaultException ex)
            {
                return new OperationResult<string>(result, false, ex.Message);
            }

        }

        public OperationResult<bool> LogOut()
        {
            bool result = false;
                 
            try
            {
                result = channel.LogOut();
                return new OperationResult<bool>(result, result);
            }
            catch (FaultException ex)
            {
                return new OperationResult<bool>(result, result, ex.Message);
            }
        }

        public OperationResult<String> Registration(string email, string login, string password, string confirmPassword)
        {
            String result = String.Empty;
            try
            {
                result = channel.Registration(email,login, password, confirmPassword);
                if (result.Contains("Error"))
                {
                    return new OperationResult<string>(result, false, result);
                }
                return new OperationResult<string>(result);
            }
            catch (FaultException ex)
            {
                return new OperationResult<string>(result, false, ex.Message);
            }
        }
    }
}
