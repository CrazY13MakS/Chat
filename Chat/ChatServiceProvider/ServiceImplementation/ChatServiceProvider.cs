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
                        x.Messages  = db.ConversationReplies.Where(y => y.ConversationId == x.Id && (y.AuthorId == curUser.Id || y.ReceiverId == curUser.Id)).Select(y => new ConversationReply()
                        {
                            Author = y.User.Login,
                            Body = y.Body,
                            ConversationId = y.ConversationId,
                            Id = y.Id,
                            SendingTime = y.SendingTime,
                            Status = (ConversationReplyStatus)y.ConversationReplyStatusId
                        }).ToList();
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

                    //var conversation = db.Conversations.FirstOrDefault(x => x.Id == conversationId);
                    //if (conversation == null)
                    //{
                    //    return new OperationResult<List<ConversationReply>>(null, false, "Error conversation Id");
                    //}
                    //var member = conversation.ConversationMembers.FirstOrDefault(x => x.Id == curUser.Id);
                    //if (member == null)
                    //{
                    //    return new OperationResult<List<ConversationReply>>(null, false, "Error conv not for you");
                    //}
                    //bool isBlocked=false;
                    //switch ((ConversationMemberStatus)member.MemberStatusId)
                    //{
                    //    case ConversationMemberStatus.None:
                    //    case ConversationMemberStatus.ReadOnly:                        
                    //    case ConversationMemberStatus.LeftConversation:                           
                    //    case ConversationMemberStatus.Blocked:
                    //        isBlocked = true;
                    //        break;
                    //    default:
                    //        break;
                    //}
                    //DateTimeOffset timeOffset= DateTimeOffset.UtcNow;
                    //if (isBlocked)
                    //{
                    //    timeOffset = member.LastStatusChanged;
                    //}
                    //var mess = conversation.ConversationReplies.Where(x => x.SendingTime >= member.Joined&&x.SendingTime<timeOffset).Select(x=>new ConversationReply() {
                    //     Author=x.User.Login,
                    //      Body=x.Body,
                    //       ConversationId=x.ConversationId,
                    //        Id=x.Id,
                    //         SendingTime=x.SendingTime,
                    //          Status=(ConversationReplyStatus)x.ConversationReplyStatusId
                    //}).ToList();
                    var mess = db.ConversationReplies.Where(x => x.ConversationId == conversationId && (x.AuthorId == curUser.Id || x.ReceiverId == curUser.Id)).Select(x => new ConversationReply()
                    {
                        Author = x.User.Login,
                        Body = x.Body,
                        ConversationId = x.ConversationId,
                        Id = x.Id,
                        SendingTime = x.SendingTime,
                        Status = (ConversationReplyStatus)x.ConversationReplyStatusId
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
            Console.WriteLine($"Send message from {curUser.Login} - to {conversationId}");
            try
            {
                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {
                    var conversationMemer = db.ConversationMembers.FirstOrDefault(x => x.MemberId == curUser.Id && x.ConversationId == conversationId);
                    if (conversationMemer == null)
                    {
                        return new OperationResult<bool>(false, false, "You are din't chat user");
                    }
                    var status = (ConversationMemberStatus)conversationMemer.MemberStatusId;
                    switch (status)
                    {
                        case ConversationMemberStatus.None:
                        case ConversationMemberStatus.Blocked:
                        case ConversationMemberStatus.ReadOnly:
                        case ConversationMemberStatus.LeftConversation:
                            return new OperationResult<bool>(false, false, $"You do not have permission to post to this Hangout. You status - {status}");
                        default:
                            break;
                    }
                    var conversation = conversationMemer.Conversation;
                    conversation.LastChange = DateTimeOffset.UtcNow;
                    DbMain.EFDbContext.ConversationReply reply = new DbMain.EFDbContext.ConversationReply()
                    {
                        AuthorId = curUser.Id,
                        Body = body,
                        ConversationId = conversationId,
                        ConversationReplyStatusId = (int)ConversationReplyStatus.Sent
                    };
                    db.ConversationReplies.Add(reply);
                    if (db.SaveChanges() > 0)
                    {
                        var members = conversation.ConversationMembers.Select(x => x.User.Id).ToList();
                        members.Remove(curUser.Id);
                        SendMessageToAllMembers(members, reply);
                        return new OperationResult<bool>(true);
                    }
                    return new OperationResult<bool>(false, false, "Send message error");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(false, false, "Internal error");
            }
        }

        private async void SendMessageToAllMembers(List<long> listUserId, DbMain.EFDbContext.ConversationReply reply)
        {
            await Task.Run(() =>
            {
                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {
                    reply.ConversationReplyStatusId = (int)ConversationReplyStatus.Received;
                    foreach (var item in listUserId)
                    {
                        reply.ReceiverId = item;
                        db.ConversationReplies.Add(reply);
                    }
                    db.SaveChanges();
                }
            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public OperationResult<Conversation> CreateDialog(string Login)
        {
            throw new NotImplementedException();
        }

        public OperationResult<Conversation> CreateConversation(string Name, bool IsOpen = false)
        {
            Console.WriteLine("Create conversation");
            try
            {
                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {
                    DbMain.EFDbContext.Conversation conversation = new DbMain.EFDbContext.Conversation()
                    {

                        AuthorId = curUser.Id,
                        Name = Name,
                        ConversationTypeId = (int)(IsOpen ? ConversationType.OpenConversation : ConversationType.PrivateConversation)

                    };
                    db.Conversations.Add(conversation);
                    if (db.SaveChanges() > 0)
                    {
                        return new OperationResult<Conversation>(new Conversation()
                        {
                            ConversationType = IsOpen ? ConversationType.OpenConversation : ConversationType.PrivateConversation,
                            Id = conversation.Id,
                            MyStatus = ConversationMemberStatus.Admin,
                            Name = Name
                        });
                    }
                    return new OperationResult<Conversation>(null, false, "Internal error");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<Conversation>(null, false, "Internal error");
            }
        }

        public OperationResult<bool> InviteFriendToConversation(string Login, long conversationId)
        {
            Console.WriteLine("Invite to conversation");
            try
            {
                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {
                    var conv = db.Conversations.FirstOrDefault(x => x.Id == conversationId);
                    if (conv == null)
                    {
                        return new OperationResult<bool>(false, false, "Conversation not found");
                    }
                    var member = conv.ConversationMembers.FirstOrDefault(x => x.MemberId == curUser.Id);
                    if (member == null)
                    {
                        return new OperationResult<bool>(false, false, "You are not in conversation");

                    }
                    var invitedUser = db.Users.FirstOrDefault(x => x.Login == Login);
                    if (invitedUser == null)
                    {
                        return new OperationResult<bool>(false, false, "User not found");
                    }
                    var contact = db.Contacts.FirstOrDefault(x => (x.AdderId == curUser.Id && x.InvitedId == invitedUser.Id) || (x.AdderId == invitedUser.Id && x.InvitedId == curUser.Id) && x.RelationTypeId == (int)RelationStatus.Friendship);
                    if (contact == null)
                    {
                        return new OperationResult<bool>(false, false, "User not your friend");
                    }
                    var convStatus = (ConversationType)conv.ConversationTypeId;
                    var memberStatus = (ConversationMemberStatus)member.MemberStatusId;
                    switch (memberStatus)
                    {
                        case ConversationMemberStatus.None:

                        case ConversationMemberStatus.Blocked:
                        case ConversationMemberStatus.ReadOnly:
                        case ConversationMemberStatus.LeftConversation:
                            return new OperationResult<bool>(false, false, "No permission to add users");
                        default:
                            break;
                    }
                    db.ConversationMembers.Add(new DbMain.EFDbContext.ConversationMember()
                    {
                        AddedId = curUser.Id,
                        ConversationId = conversationId,
                        MemberId = invitedUser.Id,
                        MemberStatusId = (int)ConversationMemberStatus.Active
                    });
                    if (db.SaveChanges() > 0)
                    {
                        return new OperationResult<bool>(true);
                    }
                    return new OperationResult<bool>(false, false, "Internal error");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(false, false, "Internal error");
            }
        }

        public OperationResult<bool> LeaveConversation(long conversationId)
        {
            try
            {
                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {
                    var conv = db.Conversations.FirstOrDefault(x => x.Id == conversationId);
                    if (conv == null)
                    {
                        return new OperationResult<bool>(false, false, "Conversation not found");
                    }
                    var member = conv.ConversationMembers.FirstOrDefault(x => x.MemberId == curUser.Id);
                    if (member == null)
                    {
                        return new OperationResult<bool>(false, false, "You are not in conversation");

                    }
                    member.MemberStatusId = (int)ConversationMemberStatus.LeftConversation;
                    if(db.SaveChanges()>0)
                    {
                        return new OperationResult<bool>(true);
                    }
                    return new OperationResult<bool>(false, false, "Internal error");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(false, false, "Internal error");
            }
        }
    }
}
