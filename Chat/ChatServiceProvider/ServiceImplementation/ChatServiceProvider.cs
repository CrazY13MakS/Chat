using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {
                    var conv = db.ConversationMembers.Include(x => x.Conversation).Include(x => x.ConversationMemberStatus).Where(x => x.MemberId == curUser.Id).Distinct().Select(x => new Conversation
                    {
                        Descriptiom = x.Conversation.Description,
                        Icon = x.Conversation.Icon,
                        ConversationType = (ConversationType)x.Conversation.ConversationTypeId,
                        LastChange = x.Conversation.LastChange,
                        Id = x.Conversation.Id,
                        MyStatus = (ConversationMemberStatus)x.MemberStatusId,
                        Name = x.Conversation.Name
                    }).ToList();

                    conv.ForEach((x) =>
                    {
                        if (x.ConversationType == ConversationType.Dialog)
                        {
                            var contact = db.Conversations.Include(y => y.User).Include(y => y.User1).FirstOrDefault(y => y.Id == x.Id);
                            if (contact.AuthorId == curUser.Id)
                            {
                                x.Partner = FromDbUserToLocal(contact.User1, x.Id);
                            }
                            else
                            {
                                x.Partner = FromDbUserToLocal(contact.User, x.Id);
                            }
                        }
                        else
                        {
                            var participants = db.ConversationMembers.Where(y => y.ConversationId == x.Id).Select(y => y.User.Login).ToList();
                            participants.Remove(curUser.Login);
                            x.ParticipantsLogin = participants;
                        }
                    });
                    return new OperationResult<List<Conversation>>(conv);
                }
            }
            catch (Exception ex)
            {

                return new OperationResult<List<Conversation>>(null, false, "InternalError GetConversations");
            }
        }

        private User FromDbUserToLocal(DbMain.EFDbContext.User user, long conversationId)
        {
            return new User()
            {
                Login = user.Login,
                Name = user.Name,
                ConversationId = conversationId,
                Icon = user.Icon,
                NetworkStatus = NetworkStatus.Unknown
            };
        }

        public OperationResult<List<ConversationReply>> GetMessages(long conversationId)
        {
            try
            {
                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {

                    var conversation = db.Conversations.FirstOrDefault(x => x.Id == conversationId);
                    if (conversation == null)
                    {
                        return new OperationResult<List<ConversationReply>>(null, false, "Error conversation Id");
                    }
                    var member = conversation.ConversationMembers.FirstOrDefault(x => x.Id == curUser.Id);
                    if (member == null)
                    {
                        return new OperationResult<List<ConversationReply>>(null, false, "Error conv not for you");
                    }
                    bool isBlocked=false;
                    switch ((ConversationMemberStatus)member.MemberStatusId)
                    {
                        case ConversationMemberStatus.None:
                        case ConversationMemberStatus.ReadOnly:                        
                        case ConversationMemberStatus.LeftConversation:                           
                        case ConversationMemberStatus.Blocked:
                            isBlocked = true;
                            break;
                        default:
                            break;
                    }
                    DateTimeOffset timeOffset= DateTimeOffset.UtcNow;
                    if (isBlocked)
                    {
                        timeOffset = member.LastStatusChanged;
                    }
                    var mess = conversation.ConversationReplies.Where(x => x.SendingTime >= member.Joined&&x.SendingTime<timeOffset).Select(x=>new ConversationReply() {
                         Author=x.User.Login,
                          Body=x.Body,
                           ConversationId=x.ConversationId,
                            Id=x.Id,
                             SendingTime=x.SendingTime,
                              Status=(ConversationReplyStatus)x.ConversationReplyStatusId
                    }).ToList();

                    return new OperationResult<List<ConversationReply>>(mess);
                }
            }
            catch (Exception)
            {
                return new OperationResult<List<ConversationReply>>(null, false, "Internal error");
            }
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

        public OperationResult<Conversation> CreateDialog(string Login)
        {
            throw new NotImplementedException();
        }
    }
}
