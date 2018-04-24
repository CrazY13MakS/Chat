using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthServiceProvider;
using DbMain.EFDbContext;

namespace ServicesProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthHost authHost = new AuthHost();
            authHost.Start();
            //using (DbMain.EFDbContext.ChatEntities a = new DbMain.EFDbContext.ChatEntities())
            //{
            //    User user = new User()
            //    {

            //    };

            //}
            //AuthServiceProvider.Model.UserAccess userAccess = new AuthServiceProvider.Model.UserAccess();
            //userAccess.Registration("crazy13maks@gmail.com", "crazy13maks", "!QAZ2wsx");
            Console.ReadKey();
        }
    }
}
