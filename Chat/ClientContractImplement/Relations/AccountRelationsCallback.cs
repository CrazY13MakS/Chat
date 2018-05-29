using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            // var notAllowedFriend = _callbackModel.FriendshipNotAllowed.FirstOrDefault(x => x.Login == login);
            var requestReceive = _callbackModel.FriendshipRequestReceive.FirstOrDefault(x => x.Login == login);
            var requestSent = _callbackModel.FriendshipRequestSend.FirstOrDefault(x => x.Login == login);
            if (friend != null)
            {
                friend.RelationStatus = relationStatus;
            }
            if (requestReceive != null)
            {
                requestReceive.RelationStatus = relationStatus;
            }
            if (requestSent != null)
            {
                requestSent.RelationStatus = relationStatus;
            }
            switch (relationStatus)
            {
                case RelationStatus.None:
                    if (requestReceive != null)
                    {
                        // _callbackModel.FriendshipNotAllowed.Remove(notAllowedFriend);
                        _callbackModel.FriendshipRequestReceive.Remove(requestReceive);
                    }
                    if (requestSent != null)
                    {
                        _callbackModel.FriendshipRequestSend.Remove(requestSent);
                    }
                    break;
                case RelationStatus.Friendship:

                    if (requestReceive != null)
                    {
                        _callbackModel.Friends.Add(requestReceive);
                        //  _callbackModel.FriendshipNotAllowed.Remove(notAllowedFriend);
                        _callbackModel.FriendshipRequestReceive.Remove(requestReceive);
                    }
                    else if (requestSent != null)
                    {
                        _callbackModel.Friends.Add(requestSent);
                        _callbackModel.FriendshipRequestSend.Remove(requestSent);
                    }
                    break;
                case RelationStatus.FriendshipRequestSent:
                    if (friend != null)
                    {
                        _callbackModel.Friends.Remove(friend);
                        _callbackModel.FriendshipRequestSend.Add(friend);
                    }
                    break;
                case RelationStatus.FrienshipRequestRecive:
                    if (friend != null)
                    {
                        _callbackModel.Friends.Remove(friend);
                        //  _callbackModel.FriendshipNotAllowed.Add(friend);
                        _callbackModel.FriendshipRequestReceive.Add(friend);
                    }
                    break;
                case RelationStatus.BlockedByMe:
                case RelationStatus.BlockedByPartner:
                case RelationStatus.BlockedBoth:
                    if (friend != null)
                    {
                        _callbackModel.Friends.Remove(friend);
                    }
                    else if (requestSent != null)
                    {
                        // _callbackModel.FriendshipNotAllowed.Remove(notAllowedFriend);
                        _callbackModel.FriendshipRequestSend.Remove(requestSent);
                    }
                    else if (requestReceive != null)
                    {
                        // _callbackModel.FriendshipNotAllowed.Remove(notAllowedFriend);
                        _callbackModel.FriendshipRequestReceive.Remove(requestReceive);
                    }
                    break;
                default:
                    break;
            }

        }
        private void RelationNone(String login)
        {
            // var reqSent =
            // _callbackModel.FriendshipRequestReceive.Remove()
        }
        public void FriendshipRequest(User user)
        {
            // _callbackModel.FriendshipNotAllowed.Add(user);
            _callbackModel.FriendshipRequestReceive.Add(user);
        }

        public void UserNetworkStatusChanged(string login, NetworkStatus status)
        {
            var user = _callbackModel.Friends.FirstOrDefault(x => x.Login == login);
            if (user != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    user.NetworkStatus = status;
                });
            }
        }
    }
}
