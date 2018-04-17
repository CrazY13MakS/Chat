using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contracts
{
    [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign)]
    public interface IAuthService
    {
        [OperationContract]
        String LogIn(String login, String password);
        
        [OperationContract]
        String Registration(String email, String login, String password, String confirmPassword);

        /// <summary>
        /// Remove token
        /// </summary>
        /// <returns>Operation success</returns>
        [OperationContract]
        bool LogOut();


    }
}
