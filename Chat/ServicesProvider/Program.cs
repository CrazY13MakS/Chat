using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthServiceProvider;
namespace ServicesProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthHost authHost = new AuthHost();
            authHost.Start();
            Console.ReadKey();
        }
    }
}
