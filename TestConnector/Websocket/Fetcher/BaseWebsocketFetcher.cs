using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestHQ;
using WebSocketSharp;

namespace TestConnector.Websocket.Fetcher
{
    internal abstract class BaseWebsocketFetcher : IDisposable
    {
        protected Dictionary<string, WebSocket> sockets = new Dictionary<string, WebSocket>();

        public void Dispose()
        {
            foreach (var ws in sockets.Values)
            {
                ws.Close();
            }
        }

        protected void Subscribe(string id, string url, string request, EventHandler<MessageEventArgs> handler)
        {
            if (sockets.ContainsKey(id))
            {
                return;
            }
            sockets[id] = new WebSocket(url);
            sockets[id].Connect();
            sockets[id].OnMessage += handler;
            sockets[id].Send(request);
        }

        protected void Unsubscribe(string id)
        {
            if (sockets.ContainsKey(id))
            {
                sockets[id].Close();
                sockets.Remove(id);
            }
        }
    }
}
