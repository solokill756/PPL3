using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PPl3.Models;
using System;
using System.Threading.Tasks;

namespace PPl3.Hubs
{
    [HubName("chat")]
    public class DemoChat : Hub
    {
        public override Task OnConnected()
        {
            var userId = Context.QueryString["userId"];
            var userName = Context.QueryString["userName"];

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                return Task.CompletedTask;
            }

            var user = new user { id = int.Parse(userId), account_name = userName }; // Tạo đối tượng user

            // Thêm ConnectionId vào nhóm đặt tên theo ID của người dùng
            Groups.Add(Context.ConnectionId, user.id.ToString());

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.QueryString["userId"];

            if (!string.IsNullOrEmpty(userId))
            {
                Groups.Remove(Context.ConnectionId, userId);
            }

            return base.OnDisconnected(stopCalled);
        }

        public void SendMessage(string receiverUserId, string message)
        {
            var userId = Context.QueryString["userId"];
            var userName = Context.QueryString["userName"];

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                return;
            }

            var senderId = userId;

            // Gửi tin nhắn đến nhóm tương ứng với ID của người nhận
            Clients.Group(receiverUserId).ReceiveMessage(senderId, receiverUserId ,  message);
        }
    }
}
