using ContractClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ChatClient.Infrastructure
{

    public class ChatTypeToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public ChatTypeToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Conversation val)
            {
                bool res = val.ConversationType == ConversationType.OpenConversation && (val.MyStatus == ConversationMemberStatus.Admin || val.MyStatus == ConversationMemberStatus.Active);
                return res ? TrueValue : FalseValue;
            }
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
            //  return TrueValue.Equals(value) ? true : false;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
