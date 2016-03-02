using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
    public class MyNancyModule : NancyModule
    {

        protected bool loggedIn()
        {
            return Context.CurrentUser != null;
        }

        protected bool loggedInAs(string username)
        {
            return loggedIn() && Context.CurrentUser.UserName == username;
        }

        protected User getUser(string username)
        {
            return UserManager.get().getUser(username);
        }

        protected User getCurUser()
        {
            return getUser(Context.CurrentUser.UserName);
        }

        protected ViewModel makeView(object data, bool condition = true)
        {
            return new ViewModel(Context, data, condition);
        }

    }
}