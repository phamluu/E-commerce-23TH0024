using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using static E_commerce_23TH0024.Lib.UserHub;
using Microsoft.AspNetCore.SignalR;

namespace E_commerce_23TH0024.Lib
{
    public class UserHub: Hub
    {
        public class UserPageInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public DateTime StartTime { get; set; }
            public UserPageInfo(string id, string name, string url, DateTime startTime)
            {
                Id = id;
                Name = name;
                Url = url;
                StartTime = startTime;
            }
        }
        private static ConcurrentDictionary<string, UserPageInfo> UserPages = new ConcurrentDictionary<string, UserPageInfo>();
        
        public async void TrackUserPage(string url, string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                await Clients.Caller.SendAsync("showError", "Id không hợp lệ.");
                return;
            }
            var connectionId = Context.ConnectionId;
            string name = Context.User != null && Context.User.Identity.IsAuthenticated ? Context.User.Identity.Name : "Chưa đăng nhập";

            var userPageInfo = new UserPageInfo(Id, name, url, DateTime.Now);

            UserPages.AddOrUpdate(
                connectionId,
                userPageInfo, 
                (key, oldValue) => userPageInfo 
            );
            await Clients.All.SendAsync("updateUserPages", UserPages);
        }

        // Phương thức gọi từ client khi người dùng ngắt kết nối
        public async void NotifyDisconnected(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                return;
            }
            if (UserPages.ContainsKey(connectionId))
            {
                UserPages.TryRemove(connectionId, out _);
                await Clients.All.SendAsync("updateUserPages", UserPages);
            }
        }
        public async void RemoveAllUsers()
        {
            UserPages.Clear();
            await Clients.All.SendAsync("updateUserPages", UserPages);
        }
       

    }
}