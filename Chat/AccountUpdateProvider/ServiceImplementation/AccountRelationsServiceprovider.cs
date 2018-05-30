using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractClient;
using ContractClient.Contracts;
using AccountRelationsProvider.Model;
using System.ServiceModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace AccountRelationsProvider.ServiceImplementation
{
    public class AccountRelationsServiceProvider : IRelations, IDisposable
    {
        DbMain.EFDbContext.ChatEntities db;
        DbMain.EFDbContext.User curUser;
        public readonly IRelationsCallback Callback;
        public AccountRelationsServiceProvider()
        {
            Console.WriteLine($"AccountUpdateServiceprovider new id= {this.GetHashCode()}");
            Callback = OperationContext.Current.GetCallbackChannel<IRelationsCallback>();
        }




        #region IRelations Implement
        public OperationResult<UserExt> Authentication(string token)
        {
            Console.WriteLine("AccountUpdateServiceprovider  Auth");
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
                    UserRelationsMain.OnlineUsers.TryAdd(curUser.Login, this);
                    UserRelationsMain.UserNetworkStatusChange(GetContacts(db, RelationStatus.Friendship), curUser.Login, NetworkStatus.OnLine);
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

        public OperationResult<bool> ChangeNetworkStatus(NetworkStatus status)
        {
            Console.WriteLine("AccountUpdateServiceprovider  ChangeNetworkStatus");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    if (status == NetworkStatus.Off || status == NetworkStatus.Unknown)
                    {
                        return new OperationResult<bool>(false, false, "Status error");
                    }
                    var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
                    user.NetworkStatusId = (int)status;
                    var friends = GetContacts(db, RelationStatus.Friendship);
                    var res = db.SaveChanges();
                    if (res == 1)
                    {
                        lock (this)
                        {
                            curUser = user;
                        }
                        UserRelationsMain.UserNetworkStatusChange(friends, curUser.Login, status == NetworkStatus.Hidden ? NetworkStatus.Off : status);
                        return new OperationResult<bool>(true);
                    }

                    return new OperationResult<bool>(false, false, "DB Error");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(false, false, "Internal error. Try again later");
            }
        }

        public OperationResult<List<User>> FindUsers(string param)
        {
            Console.WriteLine("AccountUpdateServiceprovider  FindUsers");
            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
                    var logins = db.Users.Where(x => x.Login.Contains(param) || x.Name.Contains(param)).Select(x => x.Login).ToList();
                    logins.Remove(curUser.Login);
                    var users = FromUserDbToUserClient(logins, db);
                    //var list = db.Users.Where(x => x.Login.Contains(param) || x.Name.Contains(param)).Select(x => new User()
                    //{
                    //    Login = x.Login,
                    //    Name = x.Name,
                    //    ConversationId = -1,
                    //    Icon = x.Icon,
                    //    NetworkStatus = NetworkStatus.Unknown
                    //}).ToList();

                    //list.ForEach(x =>
                    //{
                    //    var cont1 = user.Contacts.FirstOrDefault(y => y.User1.Login == x.Login);
                    //    if (cont1 != null)
                    //    {
                    //        x.RelationStatus = (RelationStatus)cont1.RelationTypeId;
                    //        return;
                    //    }
                    //    var cont2 = user.Contacts1.FirstOrDefault(y => y.User1.Login == x.Login);
                    //    if (cont2 != null)
                    //    {
                    //        x.RelationStatus = (RelationStatus)cont2.RelationTypeId;
                    //        return;
                    //    }
                    //    x.RelationStatus = RelationStatus.None;
                    //});
                    return new OperationResult<List<User>>(users);

                }
            }
            catch (Exception ex)
            {
                return new OperationResult<List<User>>(new List<User>(), false, "Internal server error");
            }
        }

        public OperationResult<User> FriendshipRequest(string body, string userLogin)
        {
            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var invited = db.Users.FirstOrDefault(x => x.Login == userLogin);
                    if (userLogin == curUser.Login || invited == null)
                    {
                        return new OperationResult<User>(null, false, "Login error");
                    }
                    var contact = db.Contacts.FirstOrDefault(x => (x.InvitedId == curUser.Id && x.AdderId == invited.Id) || (x.InvitedId == invited.Id && x.AdderId == curUser.Id));
                    if (contact != null)
                    {
                        RelationStatus status = (RelationStatus)contact.RelationTypeId;
                        bool IsInitiator = contact.AdderId == curUser.Id;
                        String message = String.Empty;
                        switch (status)
                        {
                            case RelationStatus.Friendship: message = "Friendship already confirmed"; break;
                            case RelationStatus.FriendshipRequestSent: message = IsInitiator ? "Friendship Request already Sent" : "Frienship Request already Recive"; break;
                            case RelationStatus.FrienshipRequestRecive: message = IsInitiator ? "Frienship Request already Recive" : "Friendship Request already Sent"; break;
                            case RelationStatus.BlockedByMe: message = IsInitiator ? "You blocked partner" : "You are Blocked By Partner"; break;
                            case RelationStatus.BlockedByPartner: message = IsInitiator ? "you are Blocked By Partner" : "You blocked partner"; break;
                            case RelationStatus.BlockedBoth: message = "Blocked Both"; break;
                            default:
                                break;
                        }
                        if (!String.IsNullOrEmpty(message))
                        {
                            return new OperationResult<User>(null, false, message);
                        }
                    }
                    else
                    {
                        CreateContactAndDialog(db, out contact, invited, RelationStatus.FriendshipRequestSent);
                    }

                    if (contact.AdderId == curUser.Id)
                    {
                        contact.RelationTypeId = (int)RelationStatus.FriendshipRequestSent;
                    }
                    else
                    {
                        contact.RelationTypeId = (int)RelationStatus.FrienshipRequestRecive;
                    }
                    if (db.SaveChanges() > 0)
                    {
                        UserRelationsMain.SendFrienshipRequest(invited.Login, new User()
                        {
                            ConversationId = contact.ConversationId,
                            Icon = curUser.Icon,
                            Login = curUser.Login,
                            Name = curUser.Name,
                            NetworkStatus = NetworkStatus.Unknown,
                            RelationStatus = RelationStatus.FrienshipRequestRecive
                        }, "Hello");
                        return new OperationResult<User>(new User
                        {
                            ConversationId = contact.Conversation.Id,
                            Login = userLogin,
                            Name = invited.Name,
                            RelationStatus = RelationStatus.FriendshipRequestSent,
                            Icon = invited.Icon,
                            NetworkStatus = NetworkStatus.Unknown
                        });
                    }
                    return new OperationResult<User>(null, false, "Internal error. Try again later");
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<User>(null, false, "Internal error");
            }
        }

        public OperationResult<bool> UpdateProfile(UserExt user)
        {
            try
            {

                using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
                {
                    var userDb = db.Users.FirstOrDefault(x => x.Id == curUser.Id);

                    userDb.Name = user.Name;
                    userDb.Icon = user.Icon;
                    userDb.Phone = user.Phone;
                    userDb.Birthdate = user.BirthDate;

                   // db.Users.AddOrUpdate(curUser);
                    if (db.SaveChanges() > 0)
                    {
                        curUser = userDb;
                        return new OperationResult<bool>(true);
                    }
                    return new OperationResult<bool>(false, false, "Nothing to Update");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new OperationResult<bool>(false, false, "InternalError");

        }
        public OperationResult<bool> ChangeRelationType(string login, RelationStatus status)
        {
            Console.WriteLine("AccountUpdateServiceprovider  ChangeRelationType");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    RelationStatus statusForPartner = status;
                    if (login == curUser.Login)
                    {
                        return new OperationResult<bool>(false, false, "Login error");
                    }
                    var user = db.Users.FirstOrDefault(x => x.Login == login);
                    if (user == null)
                    {
                        return new OperationResult<bool>(false, false, "User not found");
                    }
                    var contact = db.Contacts.FirstOrDefault(x => (x.AdderId == curUser.Id && x.InvitedId == user.Id) || (x.InvitedId == curUser.Id && x.AdderId == user.Id));
                    if (contact != null && contact.RelationTypeId == (int)status)
                    {
                        return new OperationResult<bool>(false, false, "Status is the same");
                    }
                    if (contact == null)
                    {
                        CreateContactAndDialog(db, out contact, user, status);
                    }
                    else
                    {
                        var convMember = user.ConversationMembers.FirstOrDefault(x => x.Conversation == contact.Conversation);
                        switch (status)
                        {
                            case RelationStatus.None:
                                convMember.MemberStatusId = (int)ConversationMemberStatus.None;
                                contact.RelationTypeId = (int)status;
                                break;
                            case RelationStatus.Friendship:
                                convMember.MemberStatusId = (int)ConversationMemberStatus.Admin;
                                contact.RelationTypeId = (int)status;
                                break;
                            case RelationStatus.FrienshipRequestRecive:
                                if (contact.AdderId == curUser.Id)
                                {
                                    contact.RelationTypeId = (int)status;
                                }
                                else
                                {
                                    contact.RelationTypeId = (int)RelationStatus.FriendshipRequestSent;
                                }
                                statusForPartner = RelationStatus.FriendshipRequestSent;
                                break;
                            case RelationStatus.BlockedByMe:
                                if (contact.AdderId == curUser.Id && (RelationStatus)contact.RelationTypeId != RelationStatus.BlockedByPartner)
                                {
                                    contact.RelationTypeId = (int)status;
                                    statusForPartner = RelationStatus.BlockedByPartner;
                                }
                                else if (contact.AdderId == curUser.Id && contact.RelationTypeId == (int)RelationStatus.BlockedByPartner
                                    || contact.InvitedId == curUser.Id && contact.RelationTypeId == (int)RelationStatus.BlockedByMe)
                                {
                                    contact.RelationTypeId = (int)RelationStatus.BlockedBoth;
                                    statusForPartner = RelationStatus.BlockedBoth;
                                }
                                else if (contact.InvitedId == curUser.Id && (RelationStatus)contact.RelationTypeId != RelationStatus.BlockedByMe)
                                {
                                    contact.RelationTypeId = (int)RelationStatus.BlockedByPartner;
                                    statusForPartner = RelationStatus.BlockedByPartner;
                                }
                                convMember.MemberStatusId = (int)ConversationMemberStatus.Blocked;
                                break;
                        }
                    }
                    var res = db.SaveChanges();
                    if (res < 1)
                    {
                        return new OperationResult<bool>(false, false, "Faild");
                    }
                    if (status == RelationStatus.Friendship)
                    {
                        UserRelationsMain.UserNetworkStatusChange(new List<string>() { curUser.Login }, user.Login, (NetworkStatus)user.NetworkStatusId);
                        UserRelationsMain.UserNetworkStatusChange(new List<string>() { user.Login }, curUser.Login, (NetworkStatus)curUser.NetworkStatusId);
                    }
                    else
                    {
                        UserRelationsMain.UserNetworkStatusChange(new List<string>() { curUser.Login }, user.Login, NetworkStatus.Unknown);
                        UserRelationsMain.UserNetworkStatusChange(new List<string>() { user.Login }, curUser.Login, NetworkStatus.Unknown);
                    }
                    UserRelationsMain.RelationTypeChanged(user.Login, curUser.Login, statusForPartner);
                    return new OperationResult<bool>(true);
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(false, false, "Internal error. Try again later");
            }
        }

        public OperationResult<List<User>> GetUsersByRelationStatus(RelationStatus relationStatus)
        {
            Console.WriteLine($"AccountUpdateServiceprovider  GetUsersByRelationStatus. Login {curUser.Login}, staus - {relationStatus} ");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    List<User> res = new List<User>();
                    List<String> contactsLogins;
                    contactsLogins = GetContacts(db, relationStatus);
                    // contactsLogins.AddRange(GetUsers(db, relationStatus));
                    switch (relationStatus)
                    {

                        case RelationStatus.Friendship:
                            res = GetFriends(db, contactsLogins);
                            break;
                        case RelationStatus.FriendshipRequestSent:
                            res = GetFriendshipRequestSendUsers(db, contactsLogins);
                            break;
                        case RelationStatus.FrienshipRequestRecive:
                            res = GetFriendshipRequestReceiveUsers(db, contactsLogins);
                            break;
                        case RelationStatus.BlockedByMe:
                            res = GetBlockedUsers(db, contactsLogins);
                            break;
                    }
                    return new OperationResult<List<User>>(res);
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<List<User>>(new List<User>(), false, "Internal error. Try again later");
            }
        }
        #endregion
        private void CreateContactAndDialog(DbMain.EFDbContext.ChatEntities db, out DbMain.EFDbContext.Contact contact, DbMain.EFDbContext.User user, RelationStatus status)
        {
            contact = new DbMain.EFDbContext.Contact()
            {
                AdderId = curUser.Id,
                InvitedId = user.Id,
                RelationTypeId = (int)status

            };
            DbMain.EFDbContext.Conversation conversation = new DbMain.EFDbContext.Conversation()
            {
                AuthorId = curUser.Id,
                PartnerId = user.Id,
                Name = $"Dialog: {curUser.Name} - {user.Name}",
                ConversationTypeId = (int)ConversationType.Dialog

            };
            contact.Conversation = conversation;
            DbMain.EFDbContext.ConversationMember member = new DbMain.EFDbContext.ConversationMember()
            {
                MemberId = curUser.Id,
                Conversation = conversation,
                MemberStatusId = (int)ConversationMemberStatus.Admin,
            };
            DbMain.EFDbContext.ConversationMember member2 = new DbMain.EFDbContext.ConversationMember()
            {
                MemberId = user.Id,
                Conversation = conversation,
                MemberStatusId = (int)(status != RelationStatus.BlockedByMe ? ConversationMemberStatus.Admin : ConversationMemberStatus.Blocked),
                AddedId = curUser.Id
            };
            db.ConversationMembers.Add(member);
            db.ConversationMembers.Add(member2);
            db.Conversations.Add(conversation);
            db.Contacts.Add(contact);
        }


        private User DbUserToCustomerUser(DbMain.EFDbContext.User user, long conversationId)
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
        //private long CreateConversation(int authorId, int? partnerId, String name = "name", String description = "description",ConversationType conversationType= ConversationType.Dialog )
        //{
        //   DbMain.EFDbContext.Conversation conversation = new DbMain.EFDbContext.Conversation()
        //   {

        //   }
        //}

        private List<String> GetContacts(DbMain.EFDbContext.ChatEntities db, RelationStatus relation)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
            List<String> result = new List<string>();
            switch (relation)
            {
                case RelationStatus.BlockedBoth:
                case RelationStatus.Friendship:
                    result = user.Contacts.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User1.Login).ToList();
                    result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User.Login).ToList());
                    break;
                case RelationStatus.FriendshipRequestSent:
                    result.AddRange(user.Contacts.Where(x => x.RelationTypeId == (int)RelationStatus.FriendshipRequestSent).Select(x => x.User1.Login));
                    result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)RelationStatus.FrienshipRequestRecive).Select(x => x.User.Login));
                    break;
                case RelationStatus.FrienshipRequestRecive:
                    result.AddRange(user.Contacts.Where(x => x.RelationTypeId == (int)RelationStatus.FrienshipRequestRecive).Select(x => x.User1.Login));
                    result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)RelationStatus.FriendshipRequestSent).Select(x => x.User.Login));
                    break;
                case RelationStatus.BlockedByMe:
                    result.AddRange(user.Contacts.Where(x => x.RelationTypeId == (int)RelationStatus.BlockedByMe).Select(x => x.User1.Login));
                    result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)RelationStatus.BlockedByPartner).Select(x => x.User.Login));
                    break;
                case RelationStatus.BlockedByPartner:
                    result.AddRange(user.Contacts.Where(x => x.RelationTypeId == (int)RelationStatus.BlockedByPartner).Select(x => x.User1.Login));
                    result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)RelationStatus.BlockedByMe).Select(x => x.User.Login));
                    break;
                default:
                    break;
            }
            //  if (relation == RelationStatus.BlockedByMe || relation == RelationStatus.FriendshipRequestSent)
            //  {
            //      result.AddRange(user.Contacts.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User1.Login));
            //      result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User1.Login))
            //  }
            //  result = user.Contacts.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User1.Login).ToList();
            //  result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User.Login).ToList());
            return result;
            // var convers = db.Contacts.Where(x => (x.AdderId == curUser.Id || x.InvitedId == curUser.Id) && (RelationStatus)x.RelationTypeId == relation).ToList();
            // var friends = convers.Select(x => x.AdderId == curUser.Id ? x.User1.Login : x.User.Login).ToList();
        }



        public OperationResult<List<User>> GetBlockedUsers()
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            using (DbMain.EFDbContext.ChatEntities db = new DbMain.EFDbContext.ChatEntities())
            {
                db.Users.FirstOrDefault(x => x.Id == curUser.Id).NetworkStatusId = (int)NetworkStatus.Off;
                db.SaveChanges();
            }
            var res = UserRelationsMain.OnlineUsers.TryRemove(curUser.Login, out AccountRelationsServiceProvider serviceprovider);
            UserRelationsMain.UserNetworkStatusChange(GetContacts(db, RelationStatus.Friendship), curUser.Login, NetworkStatus.Off);
            Console.WriteLine($"AccountUpdateServiceprovider Disposed id= {this.GetHashCode()}, login {curUser.Login}, result - {res}");
        }

        private OperationResult<List<User>> GetFriends()
        {
            Console.WriteLine("AccountUpdateServiceprovider  GetFriends");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {

                    var friendsLogins = GetContacts(db, RelationStatus.Friendship);
                    var friends = db.Users.Where(x => friendsLogins.Contains(x.Login)).ToList();

                    var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
                    var friendsToLocal = friends.Select(x => new User
                    {
                        Login = x.Login,
                        Name = x.Name,
                        Icon = x.Icon,
                        RelationStatus = RelationStatus.Friendship,
                        NetworkStatus = (NetworkStatus)x.NetworkStatusId,
                        ConversationId = db.Conversations.FirstOrDefault(c => (c.AuthorId == x.Id && c.PartnerId == user.Id) || (c.PartnerId == x.Id && c.AuthorId == user.Id))?.Id
                    }).ToList();
                    return new OperationResult<List<User>>(friendsToLocal);
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<List<User>>(new List<User>(), false, "Internal error. Try again later");
            }
        }
        private List<User> FromUserDbToUserClient(List<String> logins, DbMain.EFDbContext.ChatEntities db)
        {
            var users = db.Users.Where(x => logins.Contains(x.Login)).ToList();

            var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
            var usersToLocal = users.Select(x => new User
            {
                Login = x.Login,
                Name = x.Name,
                Icon = x.Icon,
                NetworkStatus = (NetworkStatus)x.NetworkStatusId,
                ConversationId = db.Conversations.FirstOrDefault(c => (c.AuthorId == x.Id && c.PartnerId == user.Id) || (c.PartnerId == x.Id && c.AuthorId == user.Id))?.Id
            }).ToList();
            usersToLocal.ForEach(x =>
            {
                var cont1 = user.Contacts.FirstOrDefault(y => y.User1.Login == x.Login || y.User.Login == x.Login);
                if (cont1 != null)
                {
                    x.RelationStatus = (RelationStatus)cont1.RelationTypeId;

                }
                var cont2 = user.Contacts1.FirstOrDefault(y => y.User.Login == x.Login);
                if (cont1 == null && cont2 != null)
                {
                    x.RelationStatus = (RelationStatus)cont2.RelationTypeId;
                    if (x.RelationStatus == RelationStatus.BlockedByMe)
                    {
                        x.RelationStatus = RelationStatus.BlockedByPartner;
                    }
                    else if (x.RelationStatus == RelationStatus.FriendshipRequestSent)
                    {
                        x.RelationStatus = RelationStatus.FrienshipRequestRecive;
                    }
                    else if (x.RelationStatus == RelationStatus.FrienshipRequestRecive)
                    {
                        x.RelationStatus = RelationStatus.FriendshipRequestSent;
                    }
                }
                if (x.RelationStatus != RelationStatus.Friendship)
                {
                    x.NetworkStatus = NetworkStatus.Unknown;
                }
                if (x.RelationStatus != RelationStatus.None)
                {

                }
            });

            return usersToLocal;
        }

        private List<User> GetFriends(DbMain.EFDbContext.ChatEntities db, List<String> logins)
        {
            var friends = db.Users.Where(x => logins.Contains(x.Login)).ToList();
            var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
            var friendsToLocal = friends.Select(x => new User
            {
                Login = x.Login,
                Name = x.Name,
                Icon = x.Icon,
                RelationStatus = RelationStatus.Friendship,
                NetworkStatus = (NetworkStatus)x.NetworkStatusId,
                ConversationId = db.Conversations.FirstOrDefault(c => (c.AuthorId == x.Id && c.PartnerId == user.Id) || (c.PartnerId == x.Id && c.AuthorId == user.Id)).Id
            }).ToList();
            return friendsToLocal;
        }
        private List<User> GetBlockedUsers(DbMain.EFDbContext.ChatEntities db, List<String> logins)
        {
            var users = db.Users.Where(x => logins.Contains(x.Login)).ToList();
            //  var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
            var usersToLocal = users.Select(x => new User
            {
                Login = x.Login,
                Name = x.Name,
                Icon = x.Icon,
                RelationStatus = db.Contacts
                                          .FirstOrDefault(y =>
                                          (y.AdderId == x.Id && y.InvitedId == curUser.Id)
                                          || (y.AdderId == curUser.Id && y.InvitedId == x.Id))
                                          .RelationTypeId == (int)RelationStatus.BlockedBoth ? RelationStatus.BlockedBoth : RelationStatus.BlockedByMe,
                NetworkStatus = NetworkStatus.Unknown,
                ConversationId = db.Conversations.FirstOrDefault(c => (c.AuthorId == x.Id && c.PartnerId == curUser.Id) || (c.PartnerId == x.Id && c.AuthorId == curUser.Id)).Id
            }).ToList();
            return usersToLocal;
        }
        private List<User> GetFriendshipRequestSendUsers(DbMain.EFDbContext.ChatEntities db, List<String> logins)
        {
            var users = db.Users.Where(x => logins.Contains(x.Login)).ToList();
            //  var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
            var usersToLocal = users.Select(x => new User
            {
                Login = x.Login,
                Name = x.Name,
                Icon = x.Icon,
                RelationStatus = RelationStatus.FriendshipRequestSent,
                NetworkStatus = NetworkStatus.Unknown,
                ConversationId = db.Conversations.FirstOrDefault(c => (c.AuthorId == x.Id && c.PartnerId == curUser.Id) || (c.PartnerId == x.Id && c.AuthorId == curUser.Id))?.Id
            }).ToList();

            return usersToLocal;
        }
        private List<User> GetFriendshipRequestReceiveUsers(DbMain.EFDbContext.ChatEntities db, List<String> logins)
        {
            var users = db.Users.Where(x => logins.Contains(x.Login)).ToList();
            //  var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
            var usersToLocal = users.Select(x => new User
            {
                Login = x.Login,
                Name = x.Name,
                Icon = x.Icon,
                RelationStatus = RelationStatus.FrienshipRequestRecive,
                NetworkStatus = NetworkStatus.Unknown,
                ConversationId = db.Conversations.FirstOrDefault(c => (c.AuthorId == x.Id && c.PartnerId == curUser.Id) || (c.PartnerId == x.Id && c.AuthorId == curUser.Id))?.Id
            }).ToList();

            return usersToLocal;
        }
    }
}
