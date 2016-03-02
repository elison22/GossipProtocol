using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Models
{
    public class ViewModel
    {
        public ViewModel(NancyContext ctx, object data, bool condition=true)
        {
            menuItems = new Dictionary<string, string>();

            if (ctx.CurrentUser == null ||
                ctx.CurrentUser.UserName == null ||
                ctx.CurrentUser.UserName == "")
            {
                menuItems.Add("Home", "/home");
                menuItems.Add("Create", "/create");
                menuItems.Add("Login", "/login");
            }
            else
            {
                menuItems.Add("Home", "/home");
                menuItems.Add("Account", "/account");
                menuItems.Add("Create", "/create");
                menuItems.Add("Logout", "/logout");
            }

            Data = data;
        }

        private Dictionary<string, string> menuItems;
        public string Header
        {
            get
            {
                string response = "<h2>";
                foreach (KeyValuePair<string, string> itemVal in menuItems)
                {
                    string item = "<a href=\"" + itemVal.Value + "\" >" + itemVal.Key + "</a>\n";
                    response += item;
                }
                response += "</h2>";
                return response;
            }
        }
        public object Data { get; set; }
        public bool Condition { get; set; }

    }
}