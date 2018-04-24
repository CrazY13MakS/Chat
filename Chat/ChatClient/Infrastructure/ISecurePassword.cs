using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Infrastructure
{
    interface ISecurePassword
    {
        System.Security.SecureString Password { get; }
        System.Security.SecureString ConfirmedPassword { get; }

    }
}
