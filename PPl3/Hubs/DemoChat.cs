using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPl3.Hubs
{
    [HubName("chat")]
    public class DemoChat : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Message(string GetID, string message)
        {
            Clients.All.message(GetID , message);
        }
    }
}