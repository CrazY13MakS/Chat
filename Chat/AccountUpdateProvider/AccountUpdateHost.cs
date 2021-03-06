﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AccountRelationsProvider
{
    public class AccountUpdateHost
    {
        ServiceHost host;
        public AccountUpdateHost()
        {
            host = new ServiceHost(typeof(AccountRelationsProvider.ServiceImplementation.AccountRelationsServiceProvider));
        }
        public void Open()
        {
            Console.WriteLine("AccountUpdateHost host started");
            //host.AddServiceEndpoint(new EndpointAddress("net.tcp://localhost:4000/Auth"),)
            //  host.AddServiceEndpoint(typeof(IAuthService), new NetTcpBinding(), "net.tcp://localhost:4000/Auth");
            host.Open();
        }
        public void Close()
        {
            Console.WriteLine("AccountUpdateHost host closed");
            host.Close();
        }
    }
}
