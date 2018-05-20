using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractClient;
using ContractClient.Contracts;
namespace ChatServiceProvider.ServiceImplementation
{
    public class ChatServiceProvider : IChatService, IDisposable
    {
        DbMain.EFDbContext.ChatEntities db;
        DbMain.EFDbContext.User curUser;
        public readonly IChatCallback Callback;

        public OperationResult<UserExt> Authentication(string token)
        {
            Console.WriteLine("ChatServiceProvider  Auth");
            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var access = db.AccessTokens.FirstOrDefault(x => x.IsActive && x.Token == token);
                    if (access == null)
                    {
                        return new OperationResult<UserExt>(null, false, "Faild authorization");
                    }
                    curUser = access.User;
                    if ((NetworkStatus)curUser.NetworkStatusId != NetworkStatus.Hidden)
                    {
                        access.User.NetworkStatusId = (int)NetworkStatus.OnLine;
                    }
                    db.SaveChanges();
                   // UserRelationsMain.OnlineUsers.TryAdd(curUser.Login, this);
                    return new OperationResult<UserExt>(new UserExt()
                    {
                        BirthDate = curUser.Birthdate,
                        Icon = curUser.Icon,
                        Login = curUser.Login,
                        Name = curUser.Name,
                        NetworkStatus = (NetworkStatus)curUser.NetworkStatusId,
                        Phone = curUser.Phone
                    });
                }
            }
            catch (Exception ex)
            {

                return new OperationResult<UserExt>(null, false, "Internal error. Try again later");
            }
        }

        public OperationResult<bool> Disconnect()
        {
            throw new NotImplementedException();
        }

        public OperationResult<List<Conversation>> GetConversations()
        {
            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var conv = db.ConversationMembers.First().
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public OperationResult<List<ConversationReply>> GetMessages(long conversationId)
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> LogOut()
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> SendMessage(string body, long conversationId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
