using ContractClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceProvider.Model
{
    class ChatServiceCallbackModel
    {
        public static ConcurrentDictionary<long, ServiceImplementation.ChatServiceProvider> OnlineUsers { get; set; }
        static ChatServiceCallbackModel()
        {
            OnlineUsers = new ConcurrentDictionary<long, ServiceImplementation.ChatServiceProvider>();
        }

        public async static void SendMessage(IEnumerable<long> users, ConversationReply reply)
        {
            foreach (var item in users)
            {
                if (OnlineUsers.TryGetValue(item, out ServiceImplementation.ChatServiceProvider provider))
                {
                    await Task.Run(() => provider?.Callback.IncomingMessage(reply));
                }
            }
        }

        public async static void AddingToConversation(String author, long invitedUser, Conversation conversation)
        {
            if (OnlineUsers.TryGetValue(invitedUser, out ServiceImplementation.ChatServiceProvider provider))
            {
                await Task.Run(() => provider?.Callback.AddingToConversation(conversation, author));
            }

        }
        public async static void ConversationMemberStatusChanged(long userChange, long conversationId, ConversationMemberStatus status)
        {
            if (OnlineUsers.TryGetValue(userChange, out ServiceImplementation.ChatServiceProvider provider))
            {
                await Task.Run(() => provider?.Callback.ConversationMemberStatusChanged(conversationId,status));
            }
        }
    }
}
