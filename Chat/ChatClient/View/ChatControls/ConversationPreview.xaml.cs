using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ContractClient;
namespace ChatClient.View.ChatControls
{
    /// <summary>
    /// Логика взаимодействия для ConversationPreview.xaml
    /// </summary>
    public partial class ConversationPreview : UserControl,INotifyPropertyChanged
    {

        void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;



        public Conversation Conversation
        {
            get { return (Conversation)GetValue(ConversationProperty); }
            set { SetValue(ConversationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Conversation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConversationProperty =
            DependencyProperty.Register("Conversation", typeof(Conversation), typeof(ConversationPreview), new UIPropertyMetadata(new Conversation() { }));


        public ConversationPreview()
        {
            InitializeComponent();
            DataContext = Conversation;
        }
    }
}
