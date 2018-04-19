using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AuthServiceHost.ServiceImplementation
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AuthService : ContractClient.Contracts.IAuthService,IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Dispose AuthService");
        }

        public string LogIn(string login, string password)
        {
            Console.WriteLine("LogIn");
            return "asdasd";
        }

        //public bool LogOut()
        //{
        //    Console.WriteLine("LogOut");
        //    return true;
        //}

        public string Registration(string email, string login, string password, string confirmPassword)
        {
            Console.WriteLine("Registration");
            return "dasdsadasdasdsad";
        }

        public bool SendVerificationCode(string email)
        {
            Console.WriteLine("SendVerificationCode");
            return true;
        }
    }
}
