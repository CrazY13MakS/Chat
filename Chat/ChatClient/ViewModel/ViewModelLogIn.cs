﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientContractImplement;
using ChatClient.Infrastructure;
using System.Windows.Input;
using System.Net.Mail;
using System.Security;
using System.Runtime.InteropServices;
using System.Windows.Controls;

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
            var res = authSercive.SendVerificationCode(Email);
            if (!res.IsOk)
            {
                Message = res.ErrorMessage;
            }
        }
        private bool CanExecuteSendVerifCodeCommand(object parametr)
        {
            return authSercive.CanSendCode && authSercive.IsValidMail(Email);
        }



        RelayCommand _logInCommand;
        public ICommand LogInCommand
        {
            get
            {
                if (_logInCommand == null)
                {
                    _logInCommand = new RelayCommand(ExecuteLogInCommand, CanExecuteLogInCommand);
                }
                return _logInCommand;
            }
        }

        private void ExecuteLogInCommand(object parametr)
        {

            var window = App.Current.Windows.OfType<ISecurePassword>().FirstOrDefault();
            String pass = ConvertToUnsecureString(window.Password);
           var res= authSercive.LogIn(Email, pass);

        }
        private bool CanExecuteLogInCommand(object parametr)
        {
            return true;// authSercive.IsValidMail(Email);
        }

        RelayCommand _registrationCommand;
        public ICommand RegistrationCommand
        {
            get
            {
                if (_registrationCommand == null)
                {
                    _registrationCommand = new RelayCommand(ExecuteRegistrationCommand, CanExecuteRegistrationCommand);
                }
                return _registrationCommand;
            }
        }

        private void ExecuteRegistrationCommand(object parametr)
        {
            // var a = ConvertToUnsecureString((parametr as PasswordBox).SecurePassword);
            var window = App.Current.Windows.OfType<ISecurePassword>().FirstOrDefault();
            String pass = ConvertToUnsecureString(window.Password);
            String confPass = ConvertToUnsecureString(window.ConfirmedPassword);
            var res = authSercive.Registration(Email, Login, pass, confPass);
        }
        private bool CanExecuteRegistrationCommand(object parametr)
        {
            return authSercive.IsValidMail(Email);
        }




        RelayCommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(ExecuteCloseCommand);
                }
                return _closeCommand;
            }
        }

        private void ExecuteCloseCommand(object parametr)
        {
            OnDispose();
            App.Current.Shutdown();
        }
        protected override void OnDispose()
        {
            base.OnDispose();
            if (authSercive != null)
            {
                authSercive.Dispose();
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

    }
}
