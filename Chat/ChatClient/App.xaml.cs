using ChatClient.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static String Token { get; set; }
        public static DisplayWindowHelper DisplayWindowHelper { get; protected set; } = new DisplayWindowHelper();
        public App()
        {           
            DisplayWindowHelper.RegisterWindowType<ViewModel.ViewModelLogIn, MainWindow>();
            DisplayWindowHelper.RegisterWindowType<ViewModel.ChatMainWindowViewModel, View.ChatMainWindow>();
            DisplayWindowHelper.RegisterWindowType<ViewModel.FindFriendsVM, View.FindFriends>();

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DisplayWindowHelper.ShowPresentation(new ViewModel.ViewModelLogIn());
        }
    }
}
