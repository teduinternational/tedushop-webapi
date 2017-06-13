using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeduShop.Service;
using TeduShop.Web.Infrastructure.Core;
using TeduShop.Web.Models.Common;

namespace TeduShop.Web.SignalR
{
    [Authorize]
    public class TeduShopHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();


        public static void PushToAllUsers(AnnouncementViewModel message, TeduShopHub hub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(hub);
            clients.All.addAnnouncement(message);
        }
        /// <summary>
        /// Push to a specific user
        /// </summary>
        /// <param name="who"></param>
        /// <param name="message"></param>
        public static void PushToUser(string who, AnnouncementViewModel message, TeduShopHub hub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(hub);
            foreach (var connectionId in _connections.GetConnections(who))
            {
                clients.Client(connectionId).addChatMessage(message);
            }
        }

        /// <summary>
        /// Push to list users
        /// </summary>
        /// <param name="who"></param>
        /// <param name="message"></param>
        public static void PushToUsers(string[] whos, AnnouncementViewModel message, TeduShopHub hub)
        {
            IHubConnectionContext<dynamic> clients = GetClients(hub);
            for (int i = 0; i < whos.Length; i++)
            {
                var who = whos[i];
                foreach (var connectionId in _connections.GetConnections(who))
                {
                    clients.Client(connectionId).addChatMessage(message);
                }
            }

        }
        private static IHubConnectionContext<dynamic> GetClients(TeduShopHub teduShopHub)
        {
            if (teduShopHub == null)
                return GlobalHost.ConnectionManager.GetHubContext<TeduShopHub>().Clients;
            else
                return teduShopHub.Clients;
        }

        /// <summary>
        /// Connect user to hub
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            _connections.Add(Context.User.Identity.Name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _connections.Remove(Context.User.Identity.Name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            if (!_connections.GetConnections(Context.User.Identity.Name).Contains(Context.ConnectionId))
            {
                _connections.Add(Context.User.Identity.Name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

    }
}