using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientContractImplement;
using ChatClient.Infrastructure;
using System.Windows.Input;
using System.Net.Mail;

namespace ChatClient.ViewModel
{
    class ViewModelLogIn : ViewModelBase
    {
        ClientAuthSercive authSercive = new ClientAuthSercive();
        // UserAuth _userAuth;
        //public UserAuth UserAuth
        //{
        //    get { return _userAuth; }
        //    set
        //    {
        //        if (_userAuth != value)
        //        {
        //            _userAuth = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        String _login;
        public String Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged();
                }
            }
        }
        String _email;
        public String Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        String _verifCode;
        public String VerifCode
        {
            get { return _verifCode; }
            set
            {
                if (_verifCode != value)
                {
                    _verifCode = value;
                    OnPropertyChanged();
                }
            }
        }

        String _message;
        public String Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        RelayCommand _sendVerifCode;
        public ICommand SendVerifCodeCommand
        {
            get
            {
                if (_sendVerifCode == null)
                {
                    _sendVerifCode = new RelayCommand(ExecuteSendVerifCodeCommand, CanExecuteSendVerifCodeCommand);
                }
                return _sendVerifCode;
            }
        }

        private void ExecuteSendVerifCodeCommand(object parametr)
        {
           var res= authSercive.SendVerificationCode(Email);
            if(!res.IsOk)
            {
                Message = res.ErrorMessage;
            }
        }
        private bool CanExecuteSendVerifCodeCommand(object parametr)
        {
            return authSercive.CanSendCode && authSercive.IsValidMail(Email);
        }



        RelayCommand _cogInCommand;
        public ICommand LogInCommand
        {
            get
            {
                if (_sendVerifCode == null)
                {
                    _sendVerifCode = new RelayCommand(ExecuteSendVerifCodeCommand, CanExecuteSendVerifCodeCommand);
                }
                return _sendVerifCode;
            }
        }

        private void ExecuteLogInCommand(object parametr)
        {
            var res = authSercive.SendVerificationCode(Email);
            if (!res.IsOk)
            {
                Message = res.ErrorMessage;
            }
        }
        private bool CanExecuteLogInCommand(object parametr)
        {
            return authSercive.CanSendCode && authSercive.IsValidMail(Email);
        }

    }
}
