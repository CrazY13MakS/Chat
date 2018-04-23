using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static int SecondsDelay { get; private set; } = 60;
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
            factory.Faulted += Factory_Faulted;
            channel = factory.CreateChannel();
        }

        private void Factory_Faulted(object sender, EventArgs e)
        {
            try
            {
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
        private async void UpdateCanSendCode()
        {
            await Task.Delay(TimeSpan.FromSeconds(SecondsDelay));
            lock (this)
            {
                CanSendCode = true;
            }
        }
        public OperationResult<bool> SendVerificationCode(String email)
        {
            if (!CanSendCode)
            {
                return new OperationResult<bool>(false, false, $"{(_lastSendTime.AddSeconds(SecondsDelay) - DateTime.Now)} seconds left until next try");
            }
            _lastSendTime = DateTime.Now;
            UpdateCanSendCode();
            bool result = false;
            try
            {
                //ReloadChannelIfFaulted();
                result = channel.SendVerificationCode(email);
                return new OperationResult<bool>(result, result);
            }
            catch (FaultException ex)
            {
                return new OperationResult<bool>(result, false, ex.Message);
            }
            catch (CommunicationException ex)
            {
                return new OperationResult<bool>(result, false, ex.Message);
            }
        }
        private void ReloadChannelIfFaulted()
        {
            if (factory.State != CommunicationState.Opened)
            {
                channel = factory.CreateChannel();
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

        //public OperationResult<bool> LogOut()
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = channel.LogOut();
        //        return new OperationResult<bool>(result, result);
        //    }
        //    catch (FaultException ex)
        //    {
        //        return new OperationResult<bool>(result, result, ex.Message);
        //    }
        //    catch (CommunicationException ex)
        //    {
        //        return new OperationResult<bool>(result, false, ex.Message);
        //    }
        //}

        public OperationResult<String> Registration(string email, string login, string password, string confirmPassword, String code)
        {
            String result = String.Empty;
            try
            {
                result = channel.Registration(email, login, password, confirmPassword, code);
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
