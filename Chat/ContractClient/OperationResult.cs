using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
    public class OperationResult<T>
    {
        public OperationResult(T response, bool isOk = true, String error = "")
        {
            Response = response;
            IsOk = isOk;
            ErrorMessage = error;
        }
        public T Response { get; private set; }
        public bool IsOk { get; private set; }
        public String ErrorMessage { get; private set; }
    }
}
