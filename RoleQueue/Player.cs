using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace RoleQueue
{
    public class Player
    {
        [JsonProperty]
        public int Id { get; private set; }
        [JsonProperty]
        public Role Role { get; private set; }
        // надеялся добавить возможность priority queue :(
        public bool PriorityQueue = false;
        [JsonProperty]
        public int WinCount { get; private set; } = 0;
        [JsonProperty]
        public int LossCount { get; private set; } = 0;
        //Needed Log in time beacuse really wanted to try out the DateTime type objects
        [JsonProperty]
        public string LoggedInSince { get; private set; } = DateTime.Now.ToString();
        
        public Player(int id, Role role)
        {
            Id = id;
            Role = role;
            //WinCount = 0;
            //LossCount = 0;
            //LoggedInSince = DateTime.Now.ToString();
            // Console.WriteLine(LoggedInSince.ToString());
        }

        public void AddWin()
        {
            WinCount += 1;
        }
        public void AddLoss()
        {
            LossCount += 1;
        }


    }
}
