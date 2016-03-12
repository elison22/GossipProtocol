using Nancy;
using Nancy.Authentication.Forms;
using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;

namespace GossipProtocol.Modules
{
	public class UserModule : MyNancyModule
    {
        public UserModule()
        {
            this.RequiresAuthentication();

            Get["/user/delete"] = _ =>
            {
                User toDelete = getCurUser();

                // if user is logged in, logout
                if (Context.CurrentUser != null &&
                    toDelete.UserName == Context.CurrentUser.UserName)
                    this.LogoutWithoutRedirect();

                UserManager.get().deleteUser(toDelete.UserName);
                return Response.AsRedirect("/home");
            };

            Get["/user/setid/{id}"] = _ =>
            {
                string newId = (string)_.id;

                User user = getCurUser();

                string oldFullId = user.Id.ToString();
                string oldPartId = oldFullId.Substring(0, 24);

                string newFullId = oldPartId + newId;

                try
                {
                    user.Id = Guid.Parse(newFullId);
                } catch (FormatException e)
                {
                    return View["error", makeError(newId + " must be 12 hex characters.", "home", "/home")];
                }

                return Response.AsRedirect("/home");
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