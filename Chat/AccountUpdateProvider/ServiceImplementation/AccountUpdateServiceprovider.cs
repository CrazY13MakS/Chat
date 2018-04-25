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
            try
            {
                using (db= new DbMain.EFDbContext.ChatEntities())
                {
                    var access = db.AccessTokens.FirstOrDefault(x => x.IsActive && x.Token == token);
                    if(access!=null)
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

        public OperationResult<bool> BlockUser(string login)
        {
            try
            {
                using (db = new DbMain.EFDbContext.ChatEntities())
                {
                    var user = db.Users.FirstOrDefault(x=>x.Login==login);
                    if(user==null)
                    {
                        return new OperationResult<bool>(false, false, "Login not found");

                    }
                    var contact = db.Contacts.FirstOrDefault(x => (x.AdderId == curUser.Id && x.InvitedId == user.Id) || (x.InvitedId == curUser.Id && x.AdderId == user.Id));
                    if(contact==null)
                    {
                        contact = new DbMain.EFDbContext.Contact()
                        {
                            AdderId = curUser.Id,
                            InvitedId = user.Id,
                            RelationTypeId = (int)RelationStatus.BlockedByMe

                        };
                        db.Contacts.Add(contact);
                    }
                    else
                    {
                        if(contact.)
                    }
                    if (access != null)
                    {
                        user = access.User;
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
            throw new NotImplementedException();
        }

        public OperationResult<List<User>> FindUsers(string param)
        {
            throw new NotImplementedException();
        }

        public OperationResult<User> FriendshipRequest(string body, string userLogin)
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> FrienshipResponse(string login, bool isConfirmed)
        {
            throw new NotImplementedException();
        }

        public OperationResult<List<User>> GetBlockedUsers()
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> UnBlockUser(string login)
        {
            throw new NotImplementedException();
        }

        public OperationResult<bool> UpdateProfile(UserExt user)
        {
            throw new NotImplementedException();
        }
    }
}
