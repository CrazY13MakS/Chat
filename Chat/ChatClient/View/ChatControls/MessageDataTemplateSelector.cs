using ContractClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChatClient.View.ChatControls
{
    public class MessageDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item is ConversationReply reply)
            {
                Window win =App.Current.MainWindow ;

                try
                {
                    switch (reply.Status)
                    {
                        case ConversationReplyStatus.Sent:
                        case ConversationReplyStatus.Delivered:
                        case ConversationReplyStatus.Sending:
                        case ConversationReplyStatus.SendingError:
                            return Application.Current.TryFindResource("RecivedMessage") as DataTemplate;
                        case ConversationReplyStatus.AlreadyRead:
                        case ConversationReplyStatus.Received:
                        case ConversationReplyStatus.SystemMessage:
                            return Application.Current.TryFindResource("RecivedMessage") as DataTemplate;
                        default:
                            break;
                    }
                }
                catch(Exception e)
                {
                    return null;
                }
 
            }

            return null;
        }
    }
}
