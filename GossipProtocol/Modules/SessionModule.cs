using Nancy;
using Nancy.ModelBinding;
using Nancy.Authentication.Forms;
using GossipProtocol.UserManagement;
using GossipProtocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
    public class SessionModule : MyNancyModule
    {
        public SessionModule()
        {
            Get["/login"] = _ => View["login", makeView(UserManager.get().getUsers())];
            Post["/login"] = _ =>
            {
                LoginParams loginParams = this.Bind<LoginParams>();
                User user = getUser(loginParams.Username);
                if (user == null)
                    return View["error", makeError(loginParams.Username + " is not a valid user.", "login", "/login")];

                return this.LoginAndRedirect(user.Id, fallbackRedirectUrl: "/chat");
            };

            Get["/logout"] = _ =>
            {
                return this.LogoutAndRedirect("/home");
            };
        }

        private class LoginParams
        {
            public string Username { get; set; }
        }

    }

}