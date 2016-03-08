﻿using GossipProtocol.Gossip.Message;
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
            SentState = new State();
            resetRemainingCycles();
        }

        // Inner versions of the vars below that make Nancy happy
        private List<string> UserClaims = new List<string>();
        private string Username { get; set; }
        private int defaultStartingCycles = 180;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid Id { get; set; }                        // this will form the endpoint, which will be like p5.byubrandt.com/chat/{Id}
        public List<Peer> Neighbors { get; set; }           // This is really a list of endpoints
        public State SentState { get; set; }
        public int remainingCycles { get; set; }
        
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
        public void resetRemainingCycles()
        {
            remainingCycles = defaultStartingCycles;
        }
        public bool decCycles()
        {
            remainingCycles--;
            return remainingCycles > 0;   // are there still remaining cycles?
        }
    }
}