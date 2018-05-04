using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractClient;
using ContractClient.Contracts;
using AccountRelationsProvider.Model;
using System.ServiceModel;

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
            //  Callback = OperationContext.Current.GetCallbackChannel<IRelationsCallback>();
        }

        public OperationResult<bool> Authentication(string token)
        {
            Console.WriteLine("AccountUpdateServiceprovider  Auth");
            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var access = db.AccessTokens.FirstOrDefault(x => x.IsActive && x.Token == token);
                    if (access != null)
                    {
                        curUser = access.User;
                        return new OperationResult<bool>(true);
                    }
                    access.User.NetworkStatusId = (int)NetworkStatus.OnLine;
                    db.SaveChanges();
                    UserRelationsMain.OnlineUsers.TryAdd(access.User.Login, this);
                    return new OperationResult<bool>(false, false, "Faild authorization");
                }
            }
            catch (Exception ex)
            {

                return new OperationResult<bool>(false, false, "Internal error. Try again later");
            }
        }

        private List<String> GetUsers(DbMain.EFDbContext.ChatEntities db, RelationStatus relation)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
            List<String> result = user.Contacts.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User1.Login).ToList();
            result.AddRange(user.Contacts1.Where(x => x.RelationTypeId == (int)relation).Select(x => x.User.Login).ToList());
            return result;
            // var convers = db.Contacts.Where(x => (x.AdderId == curUser.Id || x.InvitedId == curUser.Id) && (RelationStatus)x.RelationTypeId == relation).ToList();
            // var friends = convers.Select(x => x.AdderId == curUser.Id ? x.User1.Login : x.User.Login).ToList();
        }

        public OperationResult<bool> ChangeNetworkStatus(NetworkStatus status)
        {
            Console.WriteLine("AccountUpdateServiceprovider  ChangeNetworkStatus");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {

                    var user = db.Users.FirstOrDefault(x => x.Id == curUser.Id);
                    user.NetworkStatusId = (int)status;
                    var friends = GetUsers(db, RelationStatus.Friendship);
                    var res = db.SaveChanges();
                    if (res == 1)
                    {
                        UserRelationsMain.UserNetworkStatusChange(friends, curUser.Login, status);
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
        private User DbUserToCustomerUser(DbMain.EFDbContext.User user, int conversationId)
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
        public OperationResult<User> FriendshipRequest(string body, string userLogin)
        {
            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var invited = db.Users.FirstOrDefault(x => x.Login == userLogin);
                    var contact = db.Contacts.FirstOrDefault(x => (x.InvitedId == curUser.Id && x.AdderId == invited.Id) || (x.InvitedId == invited.Id && x.AdderId == curUser.Id));
                    if (contact != null)
                    {
                        RelationStatus status = (RelationStatus)contact.RelationTypeId;
                        switch (status)
                        {
                            case RelationStatus.Friendship:
                                return new OperationResult<User>(null, false, "Friendship already confirmed");
                            case RelationStatus.FriendshipRequestSent:
                                return new OperationResult<User>(null, false, "Friendship already FriendshipRequestSent");

                            case RelationStatus.FrienshipRequestRecive:
                                return new OperationResult<User>(null, false, "Friendship already FrienshipRequestRecive");

                            case RelationStatus.BlockedByMe:
                                return new OperationResult<User>(null, false, "User BlockedByMe");

                            case RelationStatus.BlockedByPartner:
                                return new OperationResult<User>(null, false, "user BlockedByPartner");

                            case RelationStatus.BlockedBoth:
                                return new OperationResult<User>(null, false, "Blocked Both");

                            default:
                                break;
                        }
                    }
                    else
                    {
                        contact = new DbMain.EFDbContext.Contact()
                        {
                            AdderId = curUser.Id,
                            InvitedId = invited.Id
                        };
                        contact.Conversation = new DbMain.EFDbContext.Conversation()
                        {
                            AuthorId = curUser.Id,
                            ConversationTypeId = (int)ConversationType.Dialog,
                            Description = "Dialog",
                            PartnerId = invited.Id,
                            Name = $"{curUser.Name} - {invited.Name}"
                        };
                        db.Contacts.Add(contact);
                    }

                    contact.RelationTypeId = (int)RelationStatus.FriendshipRequestSent;
                    if (db.SaveChanges() > 0)
                    {
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

        public OperationResult<List<User>> GetBlockedUsers()
        {
            throw new NotImplementedException();
        }



        public OperationResult<bool> UpdateProfile(UserExt user)
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> ChangeRelationType(string login, RelationStatus status)
        {
            Console.WriteLine("AccountUpdateServiceprovider  ChangeRelationType");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var user = db.Users.FirstOrDefault(x => x.Login == login);
                    if (user == null)
                    {
                        return new OperationResult<bool>(false, false, "Login not found");

                    }
                    var contact = db.Contacts.FirstOrDefault(x => (x.AdderId == curUser.Id && x.InvitedId == user.Id) || (x.InvitedId == curUser.Id && x.AdderId == user.Id));
                    if (contact == null)
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
                            Name = "Dialog"

                        };
                        contact.Conversation = conversation;
                        db.Conversations.Add(conversation);
                        db.Contacts.Add(contact);
                    }
                    else
                    {
                        contact.RelationTypeId = (int)status;
                    }
                    var res = db.SaveChanges();
                    if (res != 1)
                    {
                        return new OperationResult<bool>(false, false, "Faild");
                    }
                    return new OperationResult<bool>(true);
                }
            }
            catch (Exception ex)
            {

                return new OperationResult<bool>(false, false, "Internal error. Try again later");
            }
        }

        public void Dispose()
        {
            var res = UserRelationsMain.OnlineUsers.TryRemove(curUser.Login, out AccountRelationsServiceProvider serviceprovider);
            Console.WriteLine($"AccountUpdateServiceprovider Disposed id= {this.GetHashCode()}, login {curUser.Login}, result - {res}");
        }

        public OperationResult<List<User>> GetFriends()
        {
            Console.WriteLine("AccountUpdateServiceprovider  GetFriends");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {

                    var friendsLogins = GetUsers(db, RelationStatus.Friendship);
                    var friends = db.Users.Where(x => friendsLogins.Contains(x.Login)).ToList();

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
               // ConversationId = db.Conversations.FirstOrDefault(c => (c.AuthorId == x.Id && c.PartnerId == user.Id) || (c.PartnerId == x.Id && c.AuthorId == user.Id))?.Id
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
               if(x.RelationStatus!= RelationStatus.None)
                {

                }
            });

            return usersToLocal;
        }
        public OperationResult<List<User>> GetNotAllowedFriends()
        {
            Console.WriteLine("AccountUpdateServiceprovider  GetNotAllowedFriends");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {

                    var friendsLogins = GetUsers(db, RelationStatus.FriendshipRequestSent);
                    friendsLogins.AddRange(GetUsers(db, RelationStatus.FrienshipRequestRecive));

                    var res = FromUserDbToUserClient(friendsLogins, db);


                    return new OperationResult<List<User>>(res);
                }
            }
            catch (Exception ex)
            {
                return new OperationResult<List<User>>(new List<User>(), false, "Internal error. Try again later");
            }
        }
    }
}
