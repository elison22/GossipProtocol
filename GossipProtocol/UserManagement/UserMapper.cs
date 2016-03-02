using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Security;

namespace GossipProtocol.UserManagement
{
    public class UserMapper : IUserMapper
    {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            return UserManager.get().getUser(identifier);
        }
    }
}