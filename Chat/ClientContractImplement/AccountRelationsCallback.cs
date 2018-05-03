using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractClient;

namespace ClientContractImplement
{
    public class AccountRelationsCallback : ContractClient.Contracts.IRelationsCallback
    {
        IRelationsCallbackModel _callbackModel;
        public AccountRelationsCallback(IRelationsCallbackModel callbackModel)
        {
            _callbackModel = callbackModel;
           // _callbackModel.Friends
        }

        public void ChangeRelationType(string login, RelationStatus relationStatus)
        {
            var friend = _callbackModel.Friends.FirstOrDefault(x => x.Login == login);
            var notAllowedFriend = _callbackModel.FriendshipNotAllowed.FirstOrDefault(x => x.Login == login);
            if (friend != null)
            {
                friend.RelationStatus = relationStatus;
            }
            if (notAllowedFriend != null)
            {
                notAllowedFriend.RelationStatus = relationStatus;
            }
            switch (relationStatus)
            {
                case RelationStatus.None:
                    if (notAllowedFriend != null)
                    {
                        _callbackModel.FriendshipNotAllowed.Remove(notAllowedFriend);
                    }
                    break;
                case RelationStatus.Friendship:

                    if (notAllowedFriend != null)
                    {
                        _callbackModel.Friends.Add(notAllowedFriend);
                        _callbackModel.FriendshipNotAllowed.Remove(notAllowedFriend);
                    }
                    break;
                case RelationStatus.FriendshipRequestSent:
                case RelationStatus.FrienshipRequestRecive:
                    if (friend != null)
                    {
                        _callbackModel.Friends.Remove(friend);
                        _callbackModel.FriendshipNotAllowed.Add(friend);
                    }
                    break;
                case RelationStatus.BlockedByMe:
                case RelationStatus.BlockedByPartner:
                case RelationStatus.BlockedBoth:
                    if (friend != null)
                    {
                        _callbackModel.Friends.Remove(friend);
                    }
                    if (notAllowedFriend != null)
                    {
                        _callbackModel.FriendshipNotAllowed.Remove(notAllowedFriend);
                    }
                    break;
                default:
                    break;
            }

        }

        public void FriendshipRequest(User user)
        {
            _callbackModel.FriendshipNotAllowed.Add(user);
        }

        public void UserNetworkStatusChanged(string login, NetworkStatus status)
        {
            var user = _callbackModel.Friends.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                user.NetworkStatus = status;
            }
        }
    }
}
