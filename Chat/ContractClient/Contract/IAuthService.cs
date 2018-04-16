using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contract
{
    [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign)]
    public interface IAuthService
    {
        [OperationContract]
        String SignIn(String login, String password);
        
        [OperationContract]
        String SignUp(String email, String login, String password, String confirmPassword);
    }
}
