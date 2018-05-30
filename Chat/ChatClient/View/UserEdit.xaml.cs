using ContractClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatClient.View
{
    /// <summary>
    /// Логика взаимодействия для UserEdit.xaml
    /// </summary>
    public partial class UserEdit : Window
    {


        public UserExt User
        {
            get { return (UserExt)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(UserExt), typeof(UserEdit), new UIPropertyMetadata(new UserExt()));




        public UserEdit()
        {
            InitializeComponent();

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = File.Open(@"C:\Users\Maks\Desktop\2_NixbyKf6A.jpg", FileMode.Open);
            bi.EndInit();
            Icon = bi;
            var a = new ImageBrush();
            a.ImageSource = bi;
           // aaaa.Source =bi;
            bi.StreamSource.Close();

            // DataContext = User;
        }
        public UserEdit(UserExt user):base()
        {
            User = user;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
