using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ContractClient
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
   public class User
    {
        public long Id { get; set; }
        public String Login { get; set; }
        public String Name { get; set; }
        public int FriendsCount { get; set; }
        public String Sity { get; set; }
        public DateTime BirthDate { get; set; }
        public byte[] Icon { get; set; }
        public String Country { get; set; }
        public String Phone { get; set; }
    }
}
