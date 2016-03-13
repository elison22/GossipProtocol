using GossipProtocol.Gossip.Message;
using GossipProtocol.UserManagement;
using GossipProtocol.Writing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace GossipProtocol.Gossip
{
    public class Sender
    {
        
        public static bool SendMessage(string TargetUrl, IGossipMessage Message)
        {
            Task<bool> task = Task.Run(async () => { return await MakeAsyncRequest(TargetUrl, Message.ToJson()); });
            try
            {
                task.Wait();
            }
            catch
            {
                Write.WriteLine("Error sending message. Probably from a bad endpoint.");
            }
            return task.Result;
        }

        private static async Task<bool> MakeAsyncRequest(string Url, string Data)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage hrm = await client.PostAsync(Url, new StringContent(Data));

            return hrm.IsSuccessStatusCode;
        }

        //public static bool TestRequest(string Url, IGossipMessage Message)
        //{
        //    return SendMessage(Url, Message);
        //}
    }
}