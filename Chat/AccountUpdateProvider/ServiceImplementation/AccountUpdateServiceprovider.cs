using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractClient;
using ContractClient.Contracts;

namespace AccountUpdateProvider.ServiceImplementation
{
    public class AccountUpdateServiceprovider : IAccountUpdate
    {
        DbMain.EFDbContext.ChatEntities db;
        DbMain.EFDbContext.User curUser;
        public AccountUpdateServiceprovider()
        {

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
                    return new OperationResult<bool>(false, false, "Faild authorization");
                }
            }
            catch (Exception ex)
            {

                return new OperationResult<bool>(false, false, "Internal error. Try again later");
            }
        }


        public OperationResult<bool> ChangeNetworkStatus(NetworkStatus status)
        {
            Console.WriteLine("AccountUpdateServiceprovider  ChangeNetworkStatus");

            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    db.Users.FirstOrDefault(x => x.Id == curUser.Id).NetworkStatusId = (int)status;
                    return db.SaveChanges() == 1 ? new OperationResult<bool>(true) : new OperationResult<bool>(false, false, "DB Error");
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
                    var list = db.Users.Where(x => x.Login.Contains(param) || x.Name.Contains(param)).Select(x => new User()
                    {
                        Login = x.Login,
                        Name = x.Name,
                        ConversationId = -1,
                        Icon = x.Icon,
                        NetworkStatus = NetworkStatus.Unknown
                    }).ToList();

                    list.ForEach(x =>
                    {
                        var cont1 = user.Contacts.FirstOrDefault(y => y.User1.Login == x.Login);
                        if (cont1 != null)
                        {
                            x.RelationStatus = (RelationStatus)cont1.RelationTypeId;
                            return;
                        }
                        var cont2 = user.Contacts1.FirstOrDefault(y => y.User1.Login == x.Login);
                        if (cont2 != null)
                        {
                            x.RelationStatus = (RelationStatus)cont2.RelationTypeId;
                            return;
                        }
                        x.RelationStatus = RelationStatus.None;
                    });
                    return new OperationResult<List<User>>(list);

                }
            }
            catch (Exception ex)
            {
                return new OperationResult<List<User>>(new List<User>(), false, "Internal server error");
            }
        }

        public OperationResult<User> FriendshipRequest(string body, string userLogin)
        {
            throw new NotImplementedException();
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
    }
}
