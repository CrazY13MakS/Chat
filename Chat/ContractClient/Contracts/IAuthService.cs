using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient.Contracts
{
    [ServiceContract(ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign, SessionMode = SessionMode.Required)]
    public interface IAuthService
    {
        [OperationContract]
        String LogIn(String login, String password);

        [OperationContract]
        bool SendVerificationCode(String email);

        [OperationContract]
        String Registration(String email, String login, String password, String confirmPassword);


    }
}
