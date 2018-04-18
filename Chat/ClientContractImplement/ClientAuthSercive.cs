using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContractClient.Contracts;
namespace ClientContractImplement
{
    public class ClientAuthSercive : IDisposable
    {
        static int secondsDelay = 60;
        //Uri address = new Uri("http://localhost:4000/Auth");
        //NetTcpBinding binding = new NetTcpBinding();
        ChannelFactory<IAuthService> factory = null;
        IAuthService channel;
        DateTime _lastSendTime;
        public bool CanSendCode { get; set; }
        public ClientAuthSercive()
        {
            CanSendCode = true;
            factory = new ChannelFactory<IAuthService>("ClientAuthEndPoint");
            channel = factory.CreateChannel();
        }

        public void Dispose()
        {
            factory?.Close();
        }
        private async void UpdateCanSendCode()
        {
            await Task.Delay(TimeSpan.FromSeconds(secondsDelay));
            lock (this)
            {
                CanSendCode = true;
            }
        }
        public OperationResult<bool> SendVerificationCode(String email)
        {
            if (!CanSendCode)
            {
                return new OperationResult<bool>(false, false, $"{(_lastSendTime.AddSeconds(secondsDelay) - DateTime.Now)} seconds left until next try");
            }
            _lastSendTime = DateTime.Now;
            UpdateCanSendCode();
            bool result = false;
            try
            {
                result = channel.SendVerificationCode(email);
                return new OperationResult<bool>(result, result);
            }
            catch (FaultException ex)
            {
                return new OperationResult<bool>(result, false, ex.Message);
            }
            catch(CommunicationException ex)
            {
                return new OperationResult<bool>(result, false, ex.Message);
            }
        }

        public OperationResult<String> LogIn(string login, string password)
        {
            String result = String.Empty;
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
            catch (CommunicationException ex)
            {
                return new OperationResult<String>(result, false, ex.Message);
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
            catch (CommunicationException ex)
            {
                return new OperationResult<bool>(result, false, ex.Message);
            }
        }

        public OperationResult<String> Registration(string email, string login, string password, string confirmPassword)
        {
            String result = String.Empty;
            try
            {
                result = channel.Registration(email, login, password, confirmPassword);
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
            catch (CommunicationException ex)
            {
                return new OperationResult<String>(result, false, ex.Message);
            }
        }
        public bool IsValidMail(string emailaddress)
        {
            if (String.IsNullOrWhiteSpace(emailaddress))
            {
                return false;
            }
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
