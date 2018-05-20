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
   public class RelationTypeToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public RelationTypeToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public Visibility TrueValue { get;  set; }
        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is RelationStatus val)
            {
                String param = parameter as String;
                if (param == "Add")
                {
                    return val == RelationStatus.None ? TrueValue : FalseValue;
                }
                if(param=="Confirm")
                {
                    return val == RelationStatus.FrienshipRequestRecive ? TrueValue : FalseValue;
                }
                if(param=="Block")
                {
                 return   (val != RelationStatus.BlockedByMe)&&(val != RelationStatus.BlockedBoth) ? TrueValue : FalseValue;                    
                }
                if (param == "UnBlock")
                {
                    return (val == RelationStatus.BlockedByMe) || (val == RelationStatus.BlockedBoth) ? TrueValue : FalseValue;
                }
                if (param == "Remove")
                {
                   return val == RelationStatus.Friendship ? TrueValue : FalseValue;
                }
                if (param == "RemoveRequest")
                {
                    return val == RelationStatus.FriendshipRequestSent ? TrueValue : FalseValue;
                }
            }
            return  FalseValue;
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
