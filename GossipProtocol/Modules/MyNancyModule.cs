using GossipProtocol.Gossip;
using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using GossipProtocol.Writing;
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
            User curUser = getUser(Context.CurrentUser.UserName);

            if (curUser != null)
            {
                curUser.ResetRemainingCycles();
                Write.WriteLine("Poking from Bootstrapper.RequestStartup");
                GossipLoop.Poke(Context.CurrentUser.UserName);
            }

            return curUser;
        }

        protected ViewModel makeView(object data, bool condition = true)
        {
            return new ViewModel(Context, data, condition);
        }

        protected ViewModel makeError(string Message, string RedirectPage, string RedirectURL)
        {
            return makeView(new ErrorModel
            {
                Message = Message,
                RedirectPage = RedirectPage,
                RedirectURL = RedirectURL
            });
        }

    }
}