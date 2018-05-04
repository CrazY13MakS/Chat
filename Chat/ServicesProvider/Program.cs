using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthServiceProvider;
using DbMain.EFDbContext;
using AccountRelationsProvider;
using AccountRelationsProvider.ServiceImplementation;
using AuthServiceProvider.ServiceImplementation;

namespace ServicesProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthHost authHost = new AuthHost();
            AccountUpdateHost accHost = new AccountUpdateHost();
            authHost.Open();
            accHost.Open();
            while (true)
            {

            }


             AccountRelationsServiceProvider account = new AccountRelationsServiceProvider();
           account.Authentication("2323b814-f0fb-4489-b3be-f87e84061fc410:02:30");
          var fr=  account.GetFriends();
            var n = account.GetNotAllowedFriends();
           //    AuthService auth = new AuthService();
           //  var res1=  auth.LogIn("temp@temp.com", "!QAZ2wsx");
           //   var res2= account.Authentication(res1);
           //    var res3 = account.FriendshipRequest("sadasd", "crazy13maks");
           

            //    AuthServiceProvider.Model.UserAccess a = new AuthServiceProvider.Model.UserAccess();
            //    var res =  a.Registration("crazy13maks@gmail.com", "crazy13maks", "!QAZ2wsx");
            //   Console.WriteLine(res.Item1);
            //var key = AuthServiceProvider.Model.PasswordCrypt.GetHashFromPassword("!QAZ2wsx");
            //using (ChatEntities db = new ChatEntities())
            //{
            //    db.Database.Log = Console.WriteLine;
            //    db.Users.Add(new User
            //    {
            //        Name = "test1",
            //        Login = "test1",
            //        PasswordHash = key.Item1,
            //        PasswordSalt = key.Item2,
            //        Email = "test1@gmail.com",
            //        // NetworkStatusId=1

            //    });
            //    var r = db.Users.ToList();
            //    try
            //    {

            //        var count = db.SaveChanges();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}

            //using (DbMain.EFDbContext.ChatEntities a = new DbMain.EFDbContext.ChatEntities())
            //{
            //    User user = new User()
            //    {

            //    };

            //}
         //   Console.ReadKey();
         //  AuthServiceProvider.Model.UserAccess userAccess = new AuthServiceProvider.Model.UserAccess();
         //  userAccess.Registration("crazy13maks@gmail.com", "crazy13maks", "!QAZ2wsx");
         //   userAccess.Registration("temp@temp.com", "temp1", "!QAZ2wsx");
         //  userAccess.Registration("temp2@temp.com", "temp2", "!QAZ2wsx");
         //  userAccess.Registration("temp3@temp.com", "temp3", "!QAZ2wsx");
            Console.ReadKey();

            authHost.Close();
            accHost.Close();
        }
    }
}
