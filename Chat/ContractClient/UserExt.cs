using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContractClient
{
  public  class UserExt : User
    {
        [DataMember]
        public int FriendsCount { get; set; }

        [DataMember]
        public String Sity { get; set; }

        [DataMember]
        public DateTime BirthDate { get; set; }

        [DataMember]
        public String Country { get; set; }

        [DataMember]
        public String Phone { get; set; }

    }
}
