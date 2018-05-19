using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ContractClient;

namespace AccountRelationsProvider.Model
{
    class UserRelationsMain
    {
        public static ConcurrentDictionary<String, ServiceImplementation.AccountRelationsServiceProvider> OnlineUsers { get; set; }
        static UserRelationsMain()
        {
            OnlineUsers = new ConcurrentDictionary<string, ServiceImplementation.AccountRelationsServiceProvider>();
        }

        public async static void SendFrienshipRequest(String invited, User author, String message)
        {
            if (OnlineUsers.TryGetValue(invited, out AccountRelationsProvider.ServiceImplementation.AccountRelationsServiceProvider provider))
            {
                await Task.Run(() => provider?.Callback.FriendshipRequest(author));
            }
        }

        public async static void UserNetworkStatusChange(List<String> friends, String login, NetworkStatus networkStatus)
        {

            foreach (var item in friends)
            {
                if (OnlineUsers.TryGetValue(item, out ServiceImplementation.AccountRelationsServiceProvider provider))
                {
                    await Task.Run(() => provider?.Callback.UserNetworkStatusChanged(login, networkStatus));
                }
            }
        }
        public async static void RelationTypeChanged(String invited, String author, RelationStatus relationStatus)
        {
            if (OnlineUsers.TryGetValue(invited, out AccountRelationsProvider.ServiceImplementation.AccountRelationsServiceProvider provider))
            {
                await Task.Run(() => provider?.Callback.ChangeRelationType(author, relationStatus));
            }
        }
    }
}
