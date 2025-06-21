using Microsoft.AspNetCore.SignalR;

namespace NewsManagementSystem.Web
{
    public class DataHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendToAll(string method, params object[] args)
        {
            await Clients.All.SendAsync(method, args);
        }

        public async Task NotifyTagChanged(string action, object tag)
        {
            await SendToAll("ReceiveTagUpdate", action, tag);
        }
        public async Task BroadcastArticle(string articleHtml)
        {
            await Clients.All.SendAsync("ArticleCreated", articleHtml);
        }
 
    }
}