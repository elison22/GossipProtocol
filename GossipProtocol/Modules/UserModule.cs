using Nancy;
using Nancy.Authentication.Forms;
using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
	public class UserModule : MyNancyModule
    {
        public UserModule()
        {
            Get["/user/delete/{username}"] = _ =>
            {
                User toDelete = UserManager.get().getUser((string)_.username);

                // if it's not a valid user, go back home
                if (toDelete == null)
                    return View["home", new ViewModel(this.Context, null)];

                // if user is logged in, logout
                if (this.Context.CurrentUser != null &&
                    toDelete.UserName == this.Context.CurrentUser.UserName)
                    this.LogoutWithoutRedirect();

                UserManager.get().deleteUser(_.username);
                return View["home", new ViewModel(this.Context, null)];
            };

            Get["/user/{username}"] = _ =>
            {
                User selected = UserManager.get().getUser((string)_.username);
                
                // if it's not a valid user, go back home
                if (selected == null)
                    return View["error", new ErrorModel
                    {
                        Message = (string)_.username + " is not a valid user.",
                        RedirectPage = "home",
                        RedirectURL = "/home"
                    }];

                // if user is logged in, go to the account page
                if (this.Context.CurrentUser != null &&
                    selected.UserName == this.Context.CurrentUser.UserName)
                    return Response.AsRedirect("/account");

                // ==== something

                return View["user_adv"/*, new ViewModel(this.Context, checkins)*/];
            };
        }

        private long ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

    }
}