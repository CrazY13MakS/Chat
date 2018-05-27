using ContractClient;
using ContractClient.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientContractImplement
{
    public class ChatCustomerCallbackService : IChatCallback
    {
        IChatCallbackModel chatCallbackModel;
        public ChatCustomerCallbackService(IChatCallbackModel callbackModel)
        {
            chatCallbackModel = callbackModel;
        }
        public void AddingToConversation(Conversation conversation, string authorLogin)
        {
            chatCallbackModel.Conversations.Add(conversation);
        }



        public void ConversationMemberStatusChanged(long conversationId, ConversationMemberStatus status)
        {
            var conv = chatCallbackModel.Conversations.FirstOrDefault(x => x.Id == conversationId);
            if (conv != null)
            {
                conv.MyStatus = status;
                //var reply = new ConversationReply() { Author = whoChangedLogin, ConversationId=conversationId, SendingTime= DateTimeOffset.Now, Status= ConversationReplyStatus.Received };
                //switch (status)
                //{
                //    case ConversationMemberStatus.Blocked:
                //        reply.Body = $"{whoChangedLogin} blocked me in conversation";
                //        break;
                //    case ConversationMemberStatus.KickedOut:
                //        reply.Body = $"{whoChangedLogin} kicked me out from conversation";
                //        break;
                //    default:
                //        reply.Body = $"{whoChangedLogin} change my status to - {status}";
                //        break;
                //}
                //conv.Messages.Add(reply);
              
            }
        }

        public void IncomingMessage(ConversationReply reply)
        {
            var conv = chatCallbackModel.Conversations.FirstOrDefault(x => x.Id == reply.ConversationId);
            if (conv != null)
            {
                conv.Messages.Add(reply);
            }
        }
    }
}
