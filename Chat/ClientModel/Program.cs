using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServiceProvider.ServiceImplementation;
namespace ClientModel
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatServiceProvider.ServiceImplementation.ChatServiceProvider a = new ChatServiceProvider.ServiceImplementation.ChatServiceProvider();
            a.Authentication("cfc5ccae-3afe-4c02-bece-2670295fe6ec8:59:20");
          var c =   a.GetConversations();

        }
    }
}
