using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AuthServiceProvider.Model;

namespace AuthServiceProvider.ServiceImplementation
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults =true)]
    public class AuthService : ContractClient.Contracts.IAuthService, IDisposable
    {
        UserAccess access;
        String verifCode;
        DateTime lastCodeSent;
        public AuthService()
        {
            access = new UserAccess();
            lastCodeSent = DateTime.Now.AddMinutes(-1);
        }
        public void Dispose()
        {
            Console.WriteLine("Dispose AuthService");
            access.Dispose();
        }

        public string LogIn(string login, string password)
        {
            Console.WriteLine($"LogIn login: {login}, password: {password}");
            var result = access.LogIn(login, password);
            if (!result.Item1)
            {
                Console.WriteLine($"Login error. {result.Item2}");
                throw new FaultException(result.Item2);
            }
            return result.Item2;
        }

        //public bool LogOut()
        //{
        //    Console.WriteLine("LogOut");
        //    return true;
        //}

        public string Registration(string email, string login, string password, string confirmPassword, string code)
        {
            Console.WriteLine("Registration");
            var acessPass = access.IsValidPassword(password);
            if(!acessPass.Item1)
            {
                throw new FaultException(acessPass.Item2);
            }
            if(password!=confirmPassword)
            {
                throw new FaultException("Passwords do not match");
            }
            var res = access.Registration(email, login, password);
            if(!res.Item1)
            {
                throw new FaultException(res.Item2);
            }
            return res.Item2;
        }


        public bool SendVerificationCode(string email)
        {
            Console.WriteLine("SendVerificationCode");
            if (DateTime.Now.Subtract(lastCodeSent).TotalMinutes < 1)
            {
                return false;
            }
            var validEmail = access.IsValidEmail(email);
            if (!validEmail.Item1)
            {
                throw new FaultException(validEmail.Item2);
            }
            verifCode = access.GenerateVerificationCode();
            for (int i = 0; i < 5; i++)
            {
                if (access.SendVerificationCode(email, verifCode))
                {
                    lastCodeSent = DateTime.Now;
                    return true;
                }
            }
            return false;
        }
    }
}
