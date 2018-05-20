using System;
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
    public partial class MainWindow : Window, ChatClient.Infrastructure.ISecurePassword
    {
        public SecureString Password => GetSecureString("PassBox");

        public SecureString ConfirmedPassword => GetSecureString("PassBoxConf");

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> lst = new List<string>(){ "crazy13maks@gmail.com",
                "temp@temp.com","temp2@temp.com","temp3@temp.com","temp4@temp.com","temp5@temp.com","temp6@temp.com","temp7@temp.com","temp8@temp.com", };
            cb.ItemsSource = lst;
            cb.SelectedIndex = 0;

            var control = ContPresenter.ContentTemplate.FindName("PassBox", ContPresenter);
            if (control is PasswordBox)
            {
                (control as PasswordBox).Password = "!QAZ2wsx";
            }
        }

        SecureString GetSecureString(String name)
        {
            var control = ContPresenter.ContentTemplate.FindName(name, ContPresenter);
            if (control is PasswordBox)
            {
                return (control as PasswordBox).SecurePassword;
            }
            return new SecureString();
        }

        private void Aaa_Click(object sender, RoutedEventArgs e)
        {
            var a = this.ContPresenter.ContentTemplate.FindName("PassBox", ContPresenter);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic d = DataContext;
            d.Email = cb.SelectedItem.ToString();
          //  var control = ContPresenter.ContentTemplate.FindName("logEmail", ContPresenter);
          //  (control as TextBox).Text = cb.SelectedItem.ToString();
        }
    }
}
