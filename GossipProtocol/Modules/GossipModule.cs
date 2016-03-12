using GossipProtocol.Gossip;
using GossipProtocol.Gossip.Message;
using GossipProtocol.UserManagement;
using Nancy;
using Nancy.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.Modules
{
    public class GossipModule : MyNancyModule
    {
        public GossipModule()
        {

            Post["/chat/{id}"] = _ =>
            {
                User user = UserManager.get().getShortUser(_.id);
                if (user == null)
                {
                    //HeadResponse response = new HeadResponse(Response.Context.Response);
                    //response.StatusCode = HttpStatusCode.BadRequest;
                    //response.ReasonPhrase = _.id + " is not a valid user at this endpoint.";
                    return HttpStatusCode.BadRequest;
                }

                RequestStream requestStream = Request.Body;
                int requestLength = (int)requestStream.Length;
                byte[] requestBytes = new byte[requestLength];
                requestStream.Read(requestBytes, 0, requestLength);

                string requestBody = System.Text.Encoding.Default.GetString(requestBytes);

                WantMessage want = JsonConvert.DeserializeObject<WantMessage>(requestBody);
                RumorMessage rumor = JsonConvert.DeserializeObject<RumorMessage>(requestBody);

                if (VerifyRumor(rumor) && !user.MessageState.ReceivedMessages.Contains(rumor))
                {
                    if (!user.Neighbors.Contains(new Peer { Endpoint = rumor.EndPoint }))
                    {
                        user.AddPeer(rumor.EndPoint);
                    }

                    user.MessageState.AddMessage(rumor);
                }
                else if (VerifyWant(want))
                {
                    if (!user.Neighbors.Contains(new Peer { Endpoint = rumor.EndPoint }))
                    {
                        user.AddPeer(rumor.EndPoint);
                    }

                    List<RumorMessage> missingMessages = user.MessageState.GetMissingRumors(want.WantList);
                    foreach (RumorMessage mm in missingMessages)
                    {
                        mm.EndPoint = user.Endpoint;
                        bool success = Sender.SendMessage(want.EndPoint, mm);
                        if (success)
                        {
                            user.MessageState.AddSentTo(want.EndPoint, mm);
                        }
                    }
                }
                else
                {
                    //HeadResponse response = new HeadResponse(Response.Context.Response);
                    //response.StatusCode = HttpStatusCode.BadRequest;
                    //response.ReasonPhrase = "The following is not a valid request body:\n" + requestBody;
                    return HttpStatusCode.BadRequest;
                }

                // this is the endpoint that external nodes will use
                // it must implement the following algorithm:
                /*
                    t = getMessage();
                    if (  isRumor(t)  ) {
                         store(t)
                    } elsif ( isWant(t) ) {
                        work_queue = addWorkToQueue(t)
                        foreach w work_queue {
                          s = prepareMsg(state, w)
                          <url> = getUrl(w)
                          send(<url>, s)
                          state = update(state, s)
                        }
                    }
                */
                return HttpStatusCode.OK;
            };

        }
        
        private bool VerifyWant(WantMessage want)
        {
            return want != null &&
                    want.EndPoint != null &&
                    want.WantList != null;
        }

        private bool VerifyRumor(RumorMessage rumor)
        {
            return rumor != null &&
                    rumor.EndPoint != null &&
                    rumor.Rumor != null &&
                    rumor.Rumor.Originator != null &&
                    rumor.Rumor.Text != null &&
                    rumor.Rumor.FullId.origin != null;
        }

    }
}