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
            authHost.Open();

            //AuthServiceProvider.Model.UserAccess a = new AuthServiceProvider.Model.UserAccess();
            ////   var res =  a.Registration("crazy13maks@gmail.com", "crazy13maks", "!QAZ2wsx");

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
            // AuthServiceProvider.Model.UserAccess userAccess = new AuthServiceProvider.Model.UserAccess();
            // userAccess.Registration("crazy13maks@gmail.com", "crazy13maks", "!QAZ2wsx");
            Console.ReadKey();
            authHost.Close();
        }
    }
}
