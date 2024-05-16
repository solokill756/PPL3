using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PPl3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PPl3.Hubs
{
    [HubName("chat")]
    public class DemoChat : Hub
    {

        public override Task OnConnected()
        {

            var user = (user)HttpContext.Current.Session["user"];

            Groups.Add(Context.ConnectionId, user.id.ToString()); // Sử dụng ID của user làm group name

            return base.OnConnected();

        }

        public override Task OnDisconnected(bool stopCalled)
        {

            var user = (user)HttpContext.Current.Session["user"];

            Groups.Remove(Context.ConnectionId, user.id.ToString()); // Xóa user ra khỏi group khi ngắt kết nối

            return base.OnDisconnected(stopCalled);

        }

        public void SendMessage(string receiverUserId, string message)
        {

            var sender = (user)HttpContext.Current.Session["user"];

            Clients.Group(receiverUserId).receiveMessage(sender.id.ToString(), message); // Gửi tin nhắn đến group của receiver

        }

    }
}