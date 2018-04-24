﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,ChatClient.Infrastructure.ISecurePassword
    {
        public SecureString Password => GetSecureString("PassBox");

        public SecureString ConfirmedPassword => GetSecureString("PassBoxConf");

        public MainWindow()
        {
            InitializeComponent();  
       
        }

        SecureString GetSecureString(String name)
        {
           var control=ContPresenter.ContentTemplate.FindName(name, ContPresenter);
            if(control is PasswordBox)
            {
                return (control as PasswordBox).SecurePassword;
            }
            return new SecureString();
        }

        private void Aaa_Click(object sender, RoutedEventArgs e)
        {
           var a = this.ContPresenter.ContentTemplate.FindName("PassBox", ContPresenter);
        }
    }
}
