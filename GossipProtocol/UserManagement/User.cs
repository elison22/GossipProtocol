using GossipProtocol.Gossip.Message;
using GossipProtocol.Gossip;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipProtocol.UserManagement
{
    public class User : IUserIdentity, IComparable<User>
    {
        public User()
        {
            Neighbors = new List<Peer>();
            MessageState = new State();
            Id = Guid.NewGuid();
            defaultStartingCycles = 180;
            ResetRemainingCycles();
        }

        // Inner versions of the vars below that make Nancy happy
        private List<string> UserClaims = new List<string>();
        private string Username { get; set; }
        private int defaultStartingCycles;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid Id { get; set; }                        // this will form the endpoint, which will be like p5.byubrandt.com/chat/{Id}
        public List<Peer> Neighbors { get; set; }           // This is really a list of endpoints
        public State MessageState { get; set; }
        public int RemainingCycles { get; set; }
        public string ShortId
        {
            get
            {
                return Id.ToString().Substring(24, 12);
            }
        }
        public string Endpoint
        {
            get
            {
                return "http://p5.byubrandt.com/chat/" + ShortId;
            }
        }

        // To make Nancy happy
        public IEnumerable<string> Claims
        {
            get
            {
                return UserClaims;
            }
        }
        public string UserName
        {
            get { return Username; }
            set { Username = value; }
        }

        // public functions
        public int CompareTo(User other)
        {
            if (other == null)
                return -1;
            if (this.UserName != other.UserName)
                return -1;
            // TODO: verify equality on the other information added here
            else return 0;
        }
        public void ResetRemainingCycles()
        {
            RemainingCycles = defaultStartingCycles;
        }
        public bool DecCycles()
        {
            RemainingCycles--;
            return RemainingCycles > 0;   // are there still remaining cycles?
        }
        public void AddPeer(string Endpoint)
        {
            Peer newPeer = new Peer
            {
                Endpoint = Endpoint
            };
            Neighbors.Add(newPeer);
            MessageState.SentMessages.Add(newPeer, new List<MessageId>());
        }

    }
}