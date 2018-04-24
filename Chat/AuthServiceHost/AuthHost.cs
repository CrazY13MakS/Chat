using ContractClient.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceProvider
{
    public class AuthHost
    {
        ServiceHost host;
        public AuthHost()
        {
            host = new ServiceHost(typeof(ServiceImplementation.AuthService));
        }
        public void Start()
        {
            Console.WriteLine("Auth host started");
            //host.AddServiceEndpoint(new EndpointAddress("net.tcp://localhost:4000/Auth"),)
          //  host.AddServiceEndpoint(typeof(IAuthService), new NetTcpBinding(), "net.tcp://localhost:4000/Auth");
            host.Open();
        }
        public void Close()
        {
            Console.WriteLine("Auth host closed");
            host.Close();
        }
    }
}
