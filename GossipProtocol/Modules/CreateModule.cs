using Nancy.ModelBinding;
using Nancy.Authentication.Forms;
using GossipProtocol.Models;
using GossipProtocol.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
    public class CreateModule : MyNancyModule
    {
        public CreateModule()
        {
            Get["/create"] = _ => View["create", makeView(null)];
            Post["/create"] = _ =>
            {
                CreateParams createParams = this.Bind<CreateParams>();
                if (UserManager.get().getUser(createParams.Username) != null)
                    return View["error", makeError("A user with this username already exists.", "Create User", "/create")];
                
                User newUser = buildUser(createParams);
                UserManager.get().addUser(newUser);

                return this.LoginAndRedirect(newUser.Id, fallbackRedirectUrl:"/chat");
            };
        }

        private User buildUser(CreateParams userInfo)
        {
            User newUser = new User
            {
                FirstName = userInfo.Firstname,
                LastName = userInfo.Lastname,
                UserName = userInfo.Username,
                Id = Guid.NewGuid()
            };
            return newUser;
        }

        private class CreateParams
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Username { get; set; }
        }

    }

}