using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceProvider
{
  public  class ChatServiceHost
    {
        ServiceHost host;
        public ChatServiceHost()
        {
            host = new ServiceHost(typeof(ServiceImplementation.ChatServiceProvider));
        }
        public void Open()
        {
            Console.WriteLine("ChatServiceHost host started");
            //host.AddServiceEndpoint(new EndpointAddress("net.tcp://localhost:4000/Auth"),)
            //  host.AddServiceEndpoint(typeof(IAuthService), new NetTcpBinding(), "net.tcp://localhost:4000/Auth");
            host.Open();
        }
        public void Close()
        {
            Console.WriteLine("ChatServiceHost host closed");
            host.Close();
        }
    }
}
