using Microsoft.AspNetCore.SignalR;

namespace E_commerce_23TH0024.Hubs
{
    public class TaskHub : Hub
    {
        // Gửi broadcast từ server tới tất cả client
        public async Task SendTaskNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
